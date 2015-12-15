using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LA3DataTransfer.Model;
using LA3DataTransfer.Properties;
//using LA3_Model;

namespace LA3DataTransfer
{
    public partial class Form1 : Form
    {
        private readonly BackgroundWorker _bgwStagingData;
        private readonly BackgroundWorker _bgwAccounts;
        private readonly BackgroundWorker _bgwCollectors;
        private readonly BackgroundWorker _bgwCustomers;
        private readonly BackgroundWorker _bgwPayOffs;
        private readonly BackgroundWorker _bgwPayments;
        private readonly BackgroundWorker _bgwTheLot;
        private readonly LA_Entities _dbLa = new LA_Entities();
        private ST_Entities _dbSt = new ST_Entities();
        private readonly string _errorFileName = "";
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private bool _isTheLot;

        public Form1()
        {
            _bgwCollectors = new BackgroundWorker();
            _bgwCollectors.DoWork += bgwCollectors_DoWork;
            _bgwCollectors.RunWorkerCompleted += bgwCollectors_RunWorkerCompleted;
            _bgwCustomers = new BackgroundWorker();
            _bgwCustomers.DoWork += bgwCustomers_DoWork;
            _bgwCustomers.RunWorkerCompleted += bgwCustomers_RunWorkerCompleted;
            _bgwAccounts = new BackgroundWorker();
            _bgwAccounts.DoWork += bgwAccounts_DoWork;
            _bgwAccounts.RunWorkerCompleted += bgwAccounts_RunWorkerCompleted;
            _bgwPayments = new BackgroundWorker();
            _bgwPayments.DoWork += bgwPayments_DoWork;
            _bgwPayments.RunWorkerCompleted += bgwPayments_RunWorkerCompleted;
            _bgwPayOffs = new BackgroundWorker();
            _bgwPayOffs.DoWork += bgwPayOffs_DoWork;
            _bgwPayOffs.RunWorkerCompleted += bgwPayOffs_RunWorkerCompleted;
            _bgwTheLot = new BackgroundWorker();
            _bgwTheLot.DoWork += bgwTheLot_DoWork;
            _bgwTheLot.RunWorkerCompleted += bgwTheLot_RunWorkerCompleted;
            _bgwStagingData = new BackgroundWorker();
            _bgwStagingData.DoWork += bgwStagingData_DoWork;
            _bgwStagingData.RunWorkerCompleted += bgwStagingData_RunWorkerCompleted;

            //New Error File
            if (!Directory.Exists(Settings.Default.ErrorFolder))
                Directory.CreateDirectory(Settings.Default.ErrorFolder);
            int fileNumber = 1;
            _errorFileName = Path.Combine(Settings.Default.ErrorFolder,
                                          DateTime.Today.ToString("yyyyMMdd") + "_" +
                                          fileNumber.ToString(CultureInfo.InvariantCulture) + ".log");
            while (File.Exists(_errorFileName))
            {
                fileNumber++;
                _errorFileName = Path.Combine(Settings.Default.ErrorFolder,
                                              DateTime.Today.ToString("yyyyMMdd") + "_" +
                                              fileNumber.ToString(CultureInfo.InvariantCulture) + ".log");
            }

            InitializeComponent();
        }

        private void bgwCollectors_DoWork(object sender, DoWorkEventArgs e)
        {
            GetCollectorsFromStaging();
        }

        private void bgwCollectors_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isTheLot) return;

            _stopWatch.Stop();
            MessageBox.Show(_dbLa.Collectors.Count().ToString(CultureInfo.InvariantCulture) + @" Collectors Added: " +
                            GetMessage(_stopWatch.Elapsed));
        }

        private void bgwTheLot_DoWork(object sender, DoWorkEventArgs e)
        {
            GetCollectorsFromStaging();
            GetCustomersFromStaging();
            GetAccountsFromStaging();
            GetPaymentsFromStaging();
            GetPayOffs();
        }

        private void bgwTheLot_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _stopWatch.Stop();
            MessageBox.Show(@"The Lot: " + GetMessage(_stopWatch.Elapsed));
        }

        private void bgwCustomers_DoWork(object sender, DoWorkEventArgs e)
        {
            GetCustomersFromStaging();
        }

        private void bgwCustomers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isTheLot) return;

            _stopWatch.Stop();
            MessageBox.Show(_dbLa.Customers.Count().ToString(CultureInfo.InvariantCulture) + @" Customers Loaded.  " +
                            GetMessage(_stopWatch.Elapsed));
        }

        private void GetCollectorsFromStaging()
        {
            try
            {
                //Ref Data
                if (!_dbLa.AccountStatus.Any())
                {
                    Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Reference Data"; });
                    var ac1 = new AccountStatu { Status = "Created" };
                    var ac2 = new AccountStatu { Status = "Completed" };
                    var ac3 = new AccountStatu { Status = "Deleted" };
                    _dbLa.AccountStatus.Add(ac1);
                    _dbLa.AccountStatus.Add(ac2);
                    _dbLa.AccountStatus.Add(ac3);
                    _dbLa.SaveChanges();
                }

                Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Clearing Data"; });
                _dbLa.DeleteCollectors();

                foreach (Account a in _dbLa.Accounts)
                    _dbLa.Accounts.Remove(a);
                _dbLa.SaveChanges();

                foreach (Customer c in _dbLa.Customers)
                    _dbLa.Customers.Remove(c);
                _dbLa.SaveChanges();

                foreach (Collector c in _dbLa.Collectors)
                    _dbLa.Collectors.Remove(c);
                _dbLa.SaveChanges();

                Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Loading Collectors"; });
                foreach (ST_Collector stCollector in _dbSt.ST_Collector)
                {
                    var collector = new Collector
                        {
                            CollectorName = stCollector.name,
                            Notes = stCollector.notes ?? "",
                            OldId = stCollector.collector_id
                        };
                    _dbLa.Collectors.Add(collector);
                }
                _dbLa.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                MessageBox.Show(dbEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static string GetMessage(TimeSpan ts)
        {
            string msg = "";
            if (ts.Hours == 0 && ts.Minutes == 0)
                msg = ts.Seconds.ToString("N0") + " seconds";
            if (ts.Hours == 0 && ts.Minutes > 0)
                msg = ts.Minutes.ToString(CultureInfo.InvariantCulture) + " minutes, " + ts.Seconds.ToString("N0") +
                      " seconds";
            if (ts.Hours > 0)
                msg = ts.Hours.ToString("N0") + " hours, " + ts.Minutes.ToString(CultureInfo.InvariantCulture) +
                      " minutes, " + ts.Seconds.ToString("N0") + " ts.Seconds";
            return msg;
        }

        private int accountCodeToInt(string accountCode)
        {
            accountCode = accountCode.Trim();

            if (!accountCode.StartsWith("L"))
                throw new Exception("Not a loan");

            return int.Parse("2" + accountCode.Substring(1).Replace("/", ""));
        }

        private void btnStaging_Click(object sender, EventArgs e)
        {
            try
            {
                _stopWatch.Start();
                _bgwStagingData.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFSCollectors_Click(object sender, EventArgs e)
        {
            _stopWatch.Start();
            _bgwCollectors.RunWorkerAsync();
        }

        private void btnFSCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                _stopWatch.Start();
                _bgwCustomers.RunWorkerAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                MessageBox.Show(dbEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetCustomersFromStaging()
        {
            Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Clearing Data"; });

            foreach (Payment p in _dbLa.Payments)
                _dbLa.Payments.Remove(p);
            _dbLa.SaveChanges();

            foreach (Account a in _dbLa.Accounts)
                _dbLa.Accounts.Remove(a);
            _dbLa.SaveChanges();

            foreach (Customer c in _dbLa.Customers)
                _dbLa.Customers.Remove(c);
            _dbLa.SaveChanges();

            List<ST_Customer> stCustomers = _dbSt.ST_Customer.ToList();
            string txtAmount = " of " + stCustomers.Count.ToString(CultureInfo.InvariantCulture);
            int numberDone = 0;
            var orphanCollectorIds = new Dictionary<int, int>();
            foreach (ST_Customer stCustomer in stCustomers)
            {
                //Get New Collector ID
                var collectorID = (int)stCustomer.collector_id;
                Collector newCollector =
                    (from c in _dbLa.Collectors where c.OldId == collectorID select c).FirstOrDefault();
                if (newCollector == null)
                {
                    if (orphanCollectorIds.ContainsKey(collectorID))
                        orphanCollectorIds[collectorID]++;
                    else
                        orphanCollectorIds.Add(collectorID, 1);

                    numberDone++;
                    continue;
                }
                int newCollectorID = newCollector.Id;

                //PostCode
                string postcode = "";
                string[] line = stCustomer.address.Split('\r');
                var pcode = new Regex("[a-zA-Z]{1,2}[0-9][0-9a-zA-Z]? [0-9][a-zA-Z]{2}");
                string address = "";
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = line[i].Replace("\n", "");
                    if (line[i].Trim().Length > 0)
                    {
                        if (pcode.IsMatch(line[i]))
                        {
                            string l = line[i];
                            Match m = pcode.Match(l);
                            postcode = m.ToString();
                            l = l.Replace(postcode, "");
                            if (l.Trim() != "")
                                address += l.Trim() + Environment.NewLine;
                        }
                        else
                            address += line[i].Trim() + Environment.NewLine;
                    }
                }
                address = address.TrimEnd('\n');
                address = address.TrimEnd('\r');

                var newCustomer = new Customer
                    {
                        Address = address,
                        Collector_Id = newCollectorID,
                        Forename = stCustomer.forename,
                        Locked = false,
                        LockedByUser = "",
                        Maxloan = (stCustomer.max_loan == null) ? 0 : (int)stCustomer.max_loan,
                        Notes = stCustomer.notes ?? "",
                        PhoneNumber = stCustomer.telephone ?? "",
                        PostCode = postcode,
                        PreferredDay = (stCustomer.week_day == null) ? -1 : (int)stCustomer.week_day - 1,
                        OldId = stCustomer.cust_id,
                        StartDate = (stCustomer.start_date == null)
                                        ? new DateTime(1900, 1, 1)
                                        : (DateTime)stCustomer.start_date,
                        Surname = stCustomer.surname,
                        IsDeleted = false
                    };
                _dbLa.Customers.Add(newCustomer);

                numberDone++;
                Invoke((MethodInvoker)delegate
                    {
                        toolStripStatusLabel1.Text = @"Loading Customers: " + numberDone.ToString(CultureInfo.InvariantCulture) + txtAmount;
                    });
            }
            _dbLa.SaveChanges();

            //Log Errors
            foreach (int key in orphanCollectorIds.Keys)
                LogError(orphanCollectorIds[key].ToString(CultureInfo.InvariantCulture) + " orphan Customers.  CollectorId: " + key.ToString(CultureInfo.InvariantCulture));
        }

        private void LogError(string errorText)
        {
            var sw = new StreamWriter(_errorFileName, true, Encoding.Default);
            sw.WriteLine(errorText);
            sw.Close();
        }

        private void btnFSTheLot_Click(object sender, EventArgs e)
        {
            _stopWatch.Start();
            _isTheLot = true;
            _bgwTheLot.RunWorkerAsync();
        }

        private void btnFSAccount_Click(object sender, EventArgs e)
        {
            _stopWatch.Start();
            _bgwAccounts.RunWorkerAsync();
        }

        private void bgwAccounts_DoWork(object sender, DoWorkEventArgs e)
        {
            GetAccountsFromStaging();
        }

        private void GetAccountsFromStaging()
        {
            Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Deleting Accounts"; });

            foreach (Payment p in _dbLa.Payments)
                _dbLa.Payments.Remove(p);
            foreach (AccountStatusChange asc in _dbLa.AccountStatusChanges)
                _dbLa.AccountStatusChanges.Remove(asc);
            foreach (Account a in _dbLa.Accounts)
                _dbLa.Accounts.Remove(a);
            _dbLa.SaveChanges();

            int numberDone = 0;
            string txtAmount = " of " + _dbSt.ST_Account.Count().ToString(CultureInfo.InvariantCulture);
            var orphanAccounts = new Dictionary<int, int>();
            foreach (ST_Account stAccount in _dbSt.ST_Account)
            {
                //Find Parent Customer
                if (stAccount.cust_id != null)
                {
                    var oldCustId = (int)stAccount.cust_id;
                    Customer customer = (from c in _dbLa.Customers where c.OldId == oldCustId select c).FirstOrDefault();
                    if (customer == null)
                    {
                        if (orphanAccounts.ContainsKey(oldCustId))
                            orphanAccounts[oldCustId]++;
                        else
                            orphanAccounts.Add(oldCustId, 1);

                        numberDone++;
                        continue;
                    }

                    //Figure out invoice code details
                    //string accountID = dr["account_id"].ToString();
                    if (!stAccount.account_id.ToString(CultureInfo.InvariantCulture).StartsWith("2"))
                        throw new Exception("None loan type account");

                    string accountID = stAccount.account_id.ToString(CultureInfo.InvariantCulture).Substring(1);
                    int year = int.Parse(accountID.Substring(0, 2));
                    if (year > 20)
                        year = year + 1900;
                    else
                        year = year + 2000;
                    int invoiceNumber = int.Parse(accountID.Substring(2));

                    DateTime postedDate;
                    if (stAccount.posted_date != null)
                    {
                        postedDate = (DateTime)stAccount.posted_date;

                        if (postedDate.Year != year)
                            throw new Exception("Problem with account start year");
                    }
                    else
                        //Deal with these ones later
                        postedDate = new DateTime(1900, 1, 1);

                    var account = new Account
                        {
                            Customer_Id = customer.Id,
                            GrossValue = (double)stAccount.tot_amount,
                            InvoiceNumber = invoiceNumber,
                            LastChecked = (DateTime)stAccount.last_checked,
                            LockedByUser = "",
                            NetValue = (double)stAccount.ret_price,
                            NextPaymentDate = (DateTime)stAccount.next_payment,
                            Notes = stAccount.notes,
                            OldId = stAccount.account_id,
                            Payment = (double)stAccount.payment,
                            PaymentPeriod = (int)stAccount.period,
                            PayMonthly = stAccount.monthly,
                            PrintedForm = stAccount.printed_form,
                            StartDate = new DateTime(1900, 1, 1)
                        };

                    //Update the created date
                    AccountStatu accountStatus =
                        (from aS in _dbLa.AccountStatus where aS.Status == "Created" select aS).FirstOrDefault();
                    var asc = new AccountStatusChange
                        {
                            Account = account,
                            AccountStatus_Id = accountStatus.Id,
                            Timestamp = postedDate
                        };
                    _dbLa.AccountStatusChanges.Add(asc);

                    //Are they completed?
                    //The date will be altered later
                    if (stAccount.left_to_pay == 0)
                    {
                        AccountStatu accountStatusComp =
                            (from aS in _dbLa.AccountStatus where aS.Status == "Completed" select aS).FirstOrDefault();
                        var ascComp = new AccountStatusChange
                            {
                                Account = account,
                                AccountStatus_Id = accountStatusComp.Id,
                                Timestamp = DateTime.Now
                            };
                        _dbLa.AccountStatusChanges.Add(ascComp);
                    }

                    _dbLa.Accounts.Add(account);
                }
                numberDone++;
                Invoke(
                    (MethodInvoker)
                    delegate { toolStripStatusLabel1.Text = @"Loading Accounts: " + numberDone + txtAmount; });
            }
            _dbLa.SaveChanges();

            foreach (int key in orphanAccounts.Keys)
                LogError(orphanAccounts[key].ToString(CultureInfo.InvariantCulture) + " orphan Accounts.  CustomerID: " + key.ToString(CultureInfo.InvariantCulture));
        }

        private void bgwAccounts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isTheLot) return;

            _stopWatch.Stop();
            MessageBox.Show(_dbLa.Accounts.Count().ToString(CultureInfo.InvariantCulture) + @" Accounts Loaded.  " +
                            GetMessage(_stopWatch.Elapsed));
        }

        private void btnFSPayment_Click(object sender, EventArgs e)
        {
            _stopWatch.Start();
            _bgwPayments.RunWorkerAsync();
        }

        private void bgwPayments_DoWork(object sender, DoWorkEventArgs e)
        {
            GetPaymentsFromStaging();
        }

        private void GetPaymentsFromStaging()
        {
            try
            {
                Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Clearing Data"; });
                foreach (Payment p in _dbLa.Payments)
                    _dbLa.Payments.Remove(p);

                List<ST_Payment> stPayments = _dbSt.ST_Payment.ToList();
                string amountTxt = " of " + stPayments.Count;
                int amountDone = 0;
                int batchCount = 0;
                var orphanPayments = new Dictionary<int, int>();
                foreach (ST_Payment stPayment in stPayments)
                {
                    //Find Account
                    Account account =
                        (from a in _dbLa.Accounts where a.OldId == stPayment.account_id select a).FirstOrDefault();

                    if (account == null)
                    {
                        if (orphanPayments.ContainsKey(stPayment.account_id))
                            orphanPayments[stPayment.account_id]++;
                        else
                            orphanPayments.Add(stPayment.account_id, 1);

                        amountDone++;
                        continue;
                    }

                    var payment = new Payment
                        {
                            Account_Id = account.Id,
                            Amount = (double)stPayment.amount,
                            IsSundry = stPayment.Sundry,
                            Note = stPayment.Note ?? "",
                            PaidByAccountId = 0,
                            Timestamp = (DateTime)stPayment.date
                        };
                    _dbLa.Payments.Add(payment);

                    amountDone++;
                    batchCount++;

                    Invoke(
                        (MethodInvoker)
                        delegate
                        { toolStripStatusLabel1.Text = @"Loading Payments: " + amountDone + amountTxt; });

                    if (batchCount >= 1000)
                    {
                        Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Saving Batch"; });
                        _dbLa.SaveChanges();
                        batchCount = 0;
                    }
                }

                //Add any remaining payments
                Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Saving Final Payments"; });
                _dbLa.SaveChanges();

                //Find accounts that have a start date of 01/01/1900
                Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Sorting Account Start Dates"; });
                foreach (Account account in (from a in _dbLa.Accounts where a.StartDate.Year == 1900 select a))
                {
                    Payment payment =
                        (from p in account.Payments orderby p.Timestamp ascending select p).FirstOrDefault();
                    if (payment == null) continue;

                    //Start Date
                    account.StartDate = account.PayMonthly ? payment.Timestamp.AddMonths(account.PaymentPeriod * -1) : payment.Timestamp.AddDays(account.PaymentPeriod * -7);
                }
                _dbLa.SaveChanges();

                foreach (int key in orphanPayments.Keys)
                    LogError(orphanPayments[key] + " orphan Payments.  AccountID: " + key);
            }
            catch (DbEntityValidationException)
            {
            }
            catch (Exception)
            {
            }
        }

        private void bgwPayments_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isTheLot) return;

            _stopWatch.Stop();
            MessageBox.Show(_dbLa.Payments.Count() + @" Payments Loaded.  " +
                            GetMessage(_stopWatch.Elapsed));
        }

        private void btnFSPayoff_Click(object sender, EventArgs e)
        {
            _stopWatch.Start();
            _bgwPayOffs.RunWorkerAsync();
        }

        private void bgwPayOffs_DoWork(object sender, DoWorkEventArgs e)
        {
            GetPayOffs();
        }

        private void GetPayOffs()
        {
            Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Deleting Payoff Data"; });
            foreach (Payment p in _dbLa.Payments)
                p.PaidByAccountId = 0;
            _dbLa.SaveChanges();

            Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Loading Old Payoff Data"; });
            //DataSet dsOld = StagingDB.GetDataset("select * from Account");
            List<ST_Account> stAccounts =
                (from a in _dbSt.ST_Account where a.pay_off.Trim().Length > 0 select a).ToList();
            string txtAmount = " of " + stAccounts.Count;
            int numberDone = 0;
            foreach (ST_Account stAccount in stAccounts)
            {
                if (stAccount.pay_off.Trim().Length == 0) continue;

                //Get Payer Account
                Account payerAccount =
                    (from a in _dbLa.Accounts where a.OldId == stAccount.account_id select a).FirstOrDefault();

                if (payerAccount == null)
                {
                    Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = @"Can't locate Account"; });
                    continue;
                }

                //Get the Payee Accounts
                var payoffPayments = new List<Payment>();
                string[] oldPayeeAccountIDs = stAccount.pay_off.Split(',');
                for (int i = 0; i < oldPayeeAccountIDs.Length; i++)
                {
                    string oldPayeeAccountID = oldPayeeAccountIDs[i].Trim();
                    if (oldPayeeAccountID.Length == 0) continue;

                    //Get paid off account Account
                    int oldID = accountCodeToInt(oldPayeeAccountID);
                    Account paidOffAccount = (from a in _dbLa.Accounts where a.OldId == oldID select a).FirstOrDefault();
                    if (paidOffAccount == null)
                    {
                        LogError("Account Id: " + payerAccount.Id + " Can't find paid off account: " +
                                 oldID);
                        continue;
                    }

                    //Update the relevant payment
                    string invoiceCode = "L" + payerAccount.OldId.ToString(CultureInfo.InvariantCulture).Substring(1, 2) + "/" +
                                         payerAccount.OldId.ToString(CultureInfo.InvariantCulture).Substring(3);
                    Payment payOffPayment =
                        (from p in paidOffAccount.Payments where p.IsSundry && p.Note.IndexOf(invoiceCode, StringComparison.Ordinal) >= 0 select p)
                            .FirstOrDefault();
                    if (payOffPayment == null)
                        continue;

                    payoffPayments.Add(payOffPayment);
                }

                //Check the payoff payments
                double totalPayoff = 0;
                foreach (Payment p in payoffPayments)
                    totalPayoff += p.Amount;

                if (totalPayoff != (double)stAccount.pay_off_amount)
                {
                    var payOffAmount = (double)stAccount.pay_off_amount;
                    LogError("Account Id: " + payerAccount.Id + " paid off " +
                             payOffAmount.ToString("£0.00") + " but can only account for " +
                             totalPayoff.ToString("£0.00"));
                }

                foreach (Payment p in payoffPayments)
                    p.PaidByAccountId = payerAccount.Id;

                _dbLa.SaveChanges();

                numberDone++;
                Invoke(
                    (MethodInvoker)
                    delegate { toolStripStatusLabel1.Text = @"Payoffs: " + numberDone + txtAmount; });
            }
        }

        private void bgwPayOffs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _stopWatch.Stop();
            MessageBox.Show(@"Pay Offs Loaded.  " + GetMessage(_stopWatch.Elapsed));
        }

        #region oldMethods

        void bgwStagingData_DoWork(object sender, DoWorkEventArgs e)
        {
            //Save MDB File
            var parameter = (from p in _dbSt.Parameters where p.Type == (int)ParameterType.MDBFilename select p).FirstOrDefault();
            if (parameter == null)
            {
                parameter = new Parameter { Type = (int)ParameterType.MDBFilename };
                _dbSt.Parameters.Add(parameter);
            }
            parameter.Value = txtFilename.Text;
            _dbSt.SaveChanges();

            Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel1.Text = @"Clearing Staging Data";
            });
            _dbSt.TruncateTables1();

            //Collectors
            DataSet ds = CoreFunctions.GetDatasetFromAccess("select * from Collector", txtFilename.Text);
            string totalTxt = " of " + ds.Tables[0].Rows.Count;
            int amountDone = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var collector = new ST_Collector
                {
                    collector_id = Convert.ToInt32(dr["Collector_id"]),
                    name = dr["Name"].ToString(),
                    notes = dr["Notes"].ToString()
                };
                _dbSt.ST_Collector.Add(collector);

                amountDone++;
                Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabel1.Text = @"Collectors " + amountDone + totalTxt;
                });
            }
            _dbSt.SaveChanges();


            //Customers
            ds = CoreFunctions.GetDatasetFromAccess("select * from Customer", txtFilename.Text);
            totalTxt = " of " + ds.Tables[0].Rows.Count;
            amountDone = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var customer = new ST_Customer()
                {
                    address = dr["address"].ToString(),
                    collector_id = Convert.ToInt32(dr["collector_id"]),
                    cust_date = Convert.ToDateTime(dr["cust_date"]),
                    cust_id = Convert.ToInt32(dr["cust_id"].ToString()),
                    debt = Convert.ToDecimal(dr["debt"]),
                    forename = dr["forename"].ToString(),
                    Locked = false,
                    max_loan = Convert.ToDouble(dr["max_loan"].ToString()),
                    notes = dr["notes"].ToString(),
                    start_date = dr["start_date"] == DBNull.Value ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["start_date"]),
                    surname = dr["surname"].ToString(),
                    telephone = dr["telephone"].ToString(),
                    week_day = Convert.ToInt32(dr["week_day"])
                };
                _dbSt.ST_Customer.Add(customer);
                amountDone++;

                Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabel1.Text = @"Customers " + amountDone + totalTxt;
                });
            };
            _dbSt.SaveChanges();

            //Accounts
            ds = CoreFunctions.GetDatasetFromAccess("select * from Account", txtFilename.Text);
            totalTxt = " of " + ds.Tables[0].Rows.Count;
            amountDone = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var account = new ST_Account
                {
                    account_id = (int)dr["account_id"],
                    cust_id = Convert.ToInt32(dr["cust_id"]),
                    last_checked = Convert.ToDateTime(dr["last_checked"]),
                    left_to_pay = Convert.ToDecimal(dr["left_to_pay"]),
                    monthly = Convert.ToBoolean(dr["monthly"]),
                    next_payment = Convert.ToDateTime(dr["next_payment"]),
                    notes = dr["notes"].ToString(),
                    overdue_amount = Convert.ToDecimal(dr["overdue_amount"]),
                    pay_off = dr["pay_off"].ToString(),
                    pay_off_amount = Convert.ToDecimal(dr["pay_off_amount"]),
                    payment = Convert.ToDecimal(dr["payment"]),
                    period = Convert.ToInt16(dr["period"]),
                    posted = Convert.ToBoolean(dr["posted"]),
                    posted_date = Convert.ToDateTime(dr["posted_date"]),
                    printed_form = Convert.ToBoolean(dr["printed_form"]),
                    ret_price = Convert.ToDecimal(dr["ret_price"]),
                    tot_amount = Convert.ToDecimal(dr["tot_amount"])
                };
                _dbSt.ST_Account.Add(account);
                amountDone++;

                Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabel1.Text = @"Accounts " + amountDone + totalTxt;
                });
            }
            _dbSt.SaveChanges();

            //Payments
            ds = CoreFunctions.GetDatasetFromAccess("select * from Payment", txtFilename.Text);
            totalTxt = " of " + ds.Tables[0].Rows.Count;
            amountDone = 0;
            int batchCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var payment = new ST_Payment
                {
                    account_id = (int)dr["account_id"],
                    amount = Convert.ToDecimal(dr["amount"]),
                    date = Convert.ToDateTime(dr["date"]),
                    Left_to_pay = dr["left_to_pay"] == DBNull.Value ? 0 : Convert.ToDouble(dr["left_to_pay"]),
                    Note = dr["note"] == DBNull.Value ? "" : dr["note"].ToString(),
                    Payment_id = Convert.ToInt32(dr["payment_id"]),
                    Sundry = Convert.ToBoolean(dr["sundry"])
                };
                _dbSt.ST_Payment.Add(payment);
                amountDone++;

                //Save this batch?
                batchCount++;
                if (batchCount >= 5000)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        toolStripStatusLabel1.Text = @"Saving Payments Batch";
                    });
                    _dbSt.SaveChanges();
                    _dbSt.Dispose();
                    _dbSt = new ST_Entities();
                    batchCount = 0;
                }

                Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabel1.Text = @"Payments " + amountDone + totalTxt;
                });
            }
            _dbSt.SaveChanges();
        }

        void bgwStagingData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _stopWatch.Stop();
            MessageBox.Show(@"Staging Load Completed. " + GetMessage(_stopWatch.Elapsed));
        }

        #endregion

        private void btnFilename_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var dialogResult = dialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            txtFilename.Text = dialog.FileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var parameter = (from p in _dbSt.Parameters where p.Type == (int) ParameterType.MDBFilename select p).FirstOrDefault();
            if (parameter == null)
                txtFilename.Text = "";
            else
                txtFilename.Text = parameter.Value;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    double balance = -3000;
        //    double leftToPay = 0;
        //    double amount = 0;

        //    DataSet ds = StagingDB.GetDataset("select * from payment where account_id = '2991838'");
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        amount = Convert.ToDouble(dr["amount"].ToString());
        //        leftToPay = Convert.ToDouble(dr["left_to_pay"].ToString());
        //        if (balance <= -2000)
        //        {
        //            balance = leftToPay;
        //            balance -= amount;
        //            continue;
        //        }

        //        if (balance - amount != leftToPay)
        //            MessageBox.Show(dr["payment_id"].ToString());
        //        else
        //            balance = leftToPay;
        //    }
        //    ds.Dispose();
        //    MessageBox.Show("Eh?");
        //}
    }
}