USE [AIDE]
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertAttendance]    Script Date: 9/19/2017 1:03:28 AM ******/

-- =============================================
-- Author:		<Jhunell G. Barcenas>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_InsertAttendance]
	@EMP_ID int
	
AS

declare @COUNT tinyint = (select count(a.DATE_ENTRY) from ATTENDANCE a where a.EMP_ID = @EMP_ID and a.DATE_ENTRY = Convert(date, getdate()))

if @COUNT = 1 

begin

update ATTENDANCE set STATUS = 2 where EMP_ID = @EMP_ID and DATE_ENTRY = Convert(date, getdate())

end 

else

begin
INSERT [dbo].[ATTENDANCE]
(
	[EMP_ID],
	[DATE_ENTRY],
	[STATUS]

)
VALUES
(
	@EMP_ID,
	Convert(date, getdate()),
	2
	
)
end





