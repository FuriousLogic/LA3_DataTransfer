USE [LoanArrangerStaging_ef]
GO
/****** Object:  StoredProcedure [dbo].[TruncateTables]    Script Date: 19/08/2015 19:20:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TruncateTables] 
AS
BEGIN
	SET NOCOUNT ON;

	truncate table [dbo].[ST_Payment]
	truncate table [dbo].[ST_Account]
	truncate table [dbo].[ST_Customer]
	truncate table [dbo].[ST_Collector]

END

GO