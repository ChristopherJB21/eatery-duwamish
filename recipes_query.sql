GO
USE [EateryDB]

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Master Table For Recipe Page
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msRecipe](
	[RecipeID] [int] IDENTITY(1,1) NOT NULL,
	[DishID] [int] NOT NULL,
	[RecipeName] [varchar](200) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecipeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[msRecipe] WITH CHECK ADD FOREIGN KEY([DishID])
REFERENCES [dbo].[msDish] ([DishID])

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Master Table For Ingredients Table in Recipe Detail Page
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msRecipeIngredient](
	[RecipeIngredientID] [int] IDENTITY(1,1) NOT NULL,
	[RecipeID] [int] NOT NULL,
	[Ingredient] [varchar](200) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Unit] [varchar](200) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecipeIngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[msRecipeIngredient] WITH CHECK ADD FOREIGN KEY([RecipeID])
REFERENCES [dbo].[msRecipe] ([RecipeID])

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Master Table For Recipe Description in Recipe Detail Page
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msRecipeDescription](
	[RecipeDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[RecipeID] [int] NOT NULL,
	[Description] [text] NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecipeDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[msRecipeDescription] WITH CHECK ADD FOREIGN KEY([RecipeID])
REFERENCES [dbo].[msRecipe] ([RecipeID])