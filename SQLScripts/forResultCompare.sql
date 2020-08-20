

DECLARE 
    @query as NVARCHAR(max),
    @queryRor as NVARCHAR(max),
    @Body nvarchar(MAX),
    @cnt INT = 0

SET @query = N'insert into tPercentile select ''$FIELD$'' as Type, MAX($FIELD$) Val, Perc, null,$BUCKET$,null
 from (
 select $FIELD$,NTILE(20) OVER(ORDER BY $FIELD$ DESC) AS Perc   from  tPlans where not $FIELD$ is null and Bucket=$BUCKET$
 )t group by Perc order by Perc'




WHILE @cnt <= 29
BEGIN

	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'Earnings'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'PARTICIPANT_CONTRIB_AMT'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'EMPLR_CONTRIB_INCOME_AMT'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'ParticswithBal'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'ActPartics'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'PartRate'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'AvgPartContrib'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'AvgEmpContrib'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'Assets'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'Adminexp'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'AvgBalance'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'PartContribRate'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'EmpContribIncomeRate'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'AdminExpRate'),'$BUCKET$',@cnt) EXEC sp_executesql @Body
	SET @Body = REPLACE(REPLACE(@query,'$FIELD$', 'PercRetirees'),'$BUCKET$',@cnt) EXEC sp_executesql @Body

	SET @cnt = @cnt + 1;
END;


SELECT * FROM tPlans



INSERT INTO tPercentile SELECT Earnings as [Type], MAX(Earnings) Val, Perc, null, 1 ,null
 FROM (
	 SELECT Earnings, NTILE(100) OVER(ORDER BY Earnings ASC) AS Perc  FROM  tPlans WHERE NOT Earnings IS NULL AND Bucket=1
 )t GROUP BY Perc ORDER BY Perc