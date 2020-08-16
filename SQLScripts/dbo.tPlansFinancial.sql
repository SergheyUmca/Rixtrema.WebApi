create table dbo.tStates
(
	Id int identity(1,1),
	Code varchar(2) not null,
	[Name] varchar(255)
)
go

create unique index tStates_Id_uindex
	on dbo.tStates (Id)
go

create unique index tStates_Code_uindex
	on dbo.tStates (Code)
go

alter table dbo.tStates
	add constraint tStates_pk
		primary key nonclustered (Id)
go


INSERT INTO dbo.tStates(Code, [Name])
VALUES
		('AK',	'Alaska'),
		('AL','Alabama'),
		('AR','Arkansas'),
		('AZ','Arizona'),
		('CA','California'),
		('CO','Colorado'),
		('CT','Connecticut'),
		('DE','Delaware'),
		('FL','Florida'),
		('GA','Georgia'),
		('GU','Guam'),
		('HI','Hawaii'),
		('IA','Iowa'),
		('ID','Idaho'),
		('IL','Illinois'),
		('IN','Indiana'),
		('KS','Kansas'),
		('KY','Kentucky'),
		('LA','Louisiana'),
		('MA','Massachusetts'),
		('MD','Maryland'),
		('ME','Maine'),
		('MI','Michigan'),
		('MN','Minnesota'),
		('MO','Missouri'),
		('MS','Mississippi'),
		('MT','Montana'),
		('NC','North Carolina'),
		('ND','North Dakota'),
		('NE','Nebraska'),
		('NH','New Hampshire'),
		('NJ','New Jersey'),
		('NM','New Mexico'),
		('NV','Nevada'),
		('OH','Ohio'),
		('OK','Oklahoma'),
		('OR','Oregon'),
		('PA','Pennsylvania'),
		('RI','Rhode Island'),
		('SC','South Carolina'),
		('SD','South Dakota'),
		('TN','Tennessee'),
		('TX','Texas'),
		('UT','Utah'),
		('VA','Virginia'),
		('VI','Virgin Islands'),
		('VT','Vermont'),
		('WI','Wisconsin'),
		('WV','West Virginia'),
		('WY','Wyoming')


create table dbo.tPlans
(
	ACK_ID int identity(5,5),
	Earnings real,
	PARTICIPANT_CONTRIB_AMT real,
	EMPLR_CONTRIB_INCOME_AMT real,
	ParticswithBal real,
	ActPartics real,
	PartRate real,
	AvgPartContrib real,
	AvgEmpContrib real,
	Assets real,
	Adminexp real,
	AvgBalance real,
	PartContribRate real,
	EmpContribIncomeRate real,
	AdminExpRate real,
	PercRetirees real,
	BUSINESS_CODE varchar(50) Not NULL,
	Bucket int Not NULL,
	SPONS_DFE_MAIL_STATE VARCHAR(2) NOT NULL
)
go

create unique index tPlans_Id_uindex
	on dbo.tPlans (ACK_ID)
go

alter table dbo.tPlans
	add constraint tPlans_pk
		primary key nonclustered (ACK_ID)
go


DECLARE
	 @vCount INT = 1500000

WHILE (@vCount > 0)
BEGIN
	DECLARE
		@vRandStateNumber VARCHAR(2) =  (SELECT Code FROM dbo.tStates WHERE id = ceiling(50*Rand())),
		@vRandBuisnesCode  VARCHAR(50) = Convert(VARCHAR(50), ceiling(100000*Rand())),
		@vRandBucket INT = ceiling(29*Rand())


		INSERT INTO dbo.tPlans ( 
				SPONS_DFE_MAIL_STATE,
				Bucket,
				BUSINESS_CODE,
				Earnings, 
				PARTICIPANT_CONTRIB_AMT, 
				EMPLR_CONTRIB_INCOME_AMT,
				ParticswithBal,
				ActPartics,
				PartRate,
				AvgPartContrib,
				AvgEmpContrib,
				Assets,
				Adminexp,
				AvgBalance,
				PartContribRate,
				EmpContribIncomeRate,
				AdminExpRate,
				PercRetirees 
			)VALUES(
				@vRandStateNumber,
				@vRandBucket,
				@vRandBuisnesCode,
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND()),
				iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND())
			)

			SET @vCount -= 1
END


SELECT iif(ceiling(100*Rand()) < 5, NULL, ceiling(100*Rand()) + RAND())


SELECT *
--DELETE 
FROM dbo.tPlans