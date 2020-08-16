

CREATE TABLE [dbo].[tPercentile](
	[Type] [varchar](50) NULL,
	[Val] [real] NULL,
	[Perc] [int] NULL,
	[BUSINESS_CODE] [varchar](50) NULL,
	[Bucket] [int] NULL,
	[State] [varchar](2) NULL
) ON [PRIMARY]
GO

-- tPlans 1.5 mln
-- BC 1000
-- Bucket 0-29
-- States - ALL AL-WA