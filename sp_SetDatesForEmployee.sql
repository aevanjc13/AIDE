USE [AIDE]

-- =============================================
-- Author:		<Jhunell G. Barcenas>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


--1 = ABSENT BY DEFAULT

DECLARE @d date='01/01/2017' 
WHILE @d<'01/01/2018'
    BEGIN
        INSERT INTO [dbo].[ATTENDANCE](EMP_ID,DATE_ENTRY,STATUS)
        VALUES (1,@d,1)
        SET @d=DATEADD(DAY,1,@d)
    END

GO  
	DELETE FROM [AIDE].[dbo].[ATTENDANCE]
WHERE DATENAME(WEEKDAY,DATE_ENTRY) = 'Saturday' OR DATENAME(WEEKDAY,DATE_ENTRY) =  'Sunday'