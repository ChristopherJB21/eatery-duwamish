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

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Get all recipe by DishID
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[Recipe_Get]
	@DishId INT
AS
BEGIN
	SELECT
		RecipeID,
		DishID,
		RecipeName
	FROM dbo.msRecipe WITH(NOLOCK)
	WHERE DishID = @DishId AND AuditedActivity <> 'D'
END

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Insert or Update Recipe
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[Recipe_InsertUpdate]
	@RecipeID INT OUTPUT,
	@DishID INT,
	@RecipeName VARCHAR(200)
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msRecipe WITH(NOLOCK) WHERE RecipeID = @RecipeID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msRecipe
		SET RecipeName = @RecipeName,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE RecipeID = @RecipeID AND AuditedActivity <> 'D'
		SET @RetVal = @RecipeID
	END
	ELSE
	BEGIN
		INSERT INTO msRecipe 
		(DishID, RecipeName, AuditedActivity, AuditedTime)
		VALUES
		(@DishID, @RecipeName, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @RecipeID = @RetVal
END

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Delete Recipe
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[Recipe_Delete]
	@RecipeIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msRecipe
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE RecipeID IN (SELECT value FROM fn_General_Split(@RecipeIDs, ','))
END

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Get all recipe ingredient by RecipeID
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RecipeIngredient_Get]
	@RecipeId INT
AS
BEGIN
	SELECT
		RecipeIngredientID,
		RecipeID,
		Ingredient,
		Quantity,
		Unit
	FROM dbo.msRecipeIngredient WITH(NOLOCK)
	WHERE RecipeID = @RecipeId AND AuditedActivity <> 'D'
END

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Insert or Update Recipe
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RecipeIngredient_InsertUpdate]
	@RecipeIngredientID INT OUTPUT,
	@RecipeID INT,
	@Ingredient VARCHAR(200),
	@Quantity INT,
	@Unit VARCHAR(200)
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msRecipeIngredient WITH(NOLOCK) WHERE RecipeIngredientID = @RecipeIngredientID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msRecipeIngredient
		SET Ingredient = @Ingredient,
			Quantity = @Quantity,
			Unit = @Unit,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE RecipeIngredientID = @RecipeIngredientID AND AuditedActivity <> 'D'
		SET @RetVal = @RecipeIngredientID
	END
	ELSE
	BEGIN
		INSERT INTO msRecipeIngredient 
		(RecipeID, Ingredient, Quantity, Unit, AuditedActivity, AuditedTime)
		VALUES
		(@RecipeID, @Ingredient, @Quantity, @Unit, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @RecipeIngredientID = @RetVal
END

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Delete Recipe Ingredient
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RecipeIngredient_Delete]
	@RecipeIngredientIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msRecipeIngredient
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE RecipeIngredientID IN (SELECT value FROM fn_General_Split(@RecipeIngredientIDs, ','))
END

SELECT * FROM dbo.msRecipeDescription

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Get all recipe description by RecipeID
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RecipeDescription_Get]
	@RecipeId INT
AS
BEGIN
	SELECT
		RecipeDescriptionID,
		RecipeID,
		Description
	FROM dbo.msRecipeDescription WITH(NOLOCK)
	WHERE RecipeID = @RecipeId AND AuditedActivity <> 'D'
END

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Insert or Update Recipe
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RecipeDescription_InsertUpdate]
	@RecipeDescriptionID INT OUTPUT,
	@RecipeID INT,
	@Description TEXT
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msRecipeDescription WITH(NOLOCK) WHERE RecipeDescriptionID = @RecipeDescriptionID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msRecipeDescription
		SET Description = @Description,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE RecipeDescriptionID = @RecipeDescriptionID AND AuditedActivity <> 'D'
		SET @RetVal = @RecipeDescriptionID
	END
	ELSE
	BEGIN
		INSERT INTO msRecipeDescription
		(RecipeID, Description, AuditedActivity, AuditedTime)
		VALUES
		(@RecipeID, @Description, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @RecipeDescriptionID = @RetVal
END

/**
 * Created by: Christopher Gavra Reswara
 * Date: 26 Maret 2022
 * Purpose: Delete Recipe Ingredient
 */
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RecipeDescription_Delete]
	@RecipeDescriptionIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msRecipeDescription
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE RecipeDescriptionID IN (SELECT value FROM fn_General_Split(@RecipeDescriptionIDs, ','))
END
