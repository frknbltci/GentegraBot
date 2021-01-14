
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/8/2020 3:27:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[Password] [nvarchar](50) NULL,
	[Role] [bit] NULL,
	[FileURL] [nvarchar](max) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [UserName], [IsActive], [Password], [Role], [FileURL]) VALUES (1, N'turan', 1, N'turan', 1, NULL)
INSERT [dbo].[Users] ([ID], [UserName], [IsActive], [Password], [Role], [FileURL]) VALUES (2, N'eşref', 1, N'eşref', 1, N'C:\Users\fbalt\source\repos\Ppbot2\Ppbot\PpBot\PpBot.UI\Assets\Content\esref\')
INSERT [dbo].[Users] ([ID], [UserName], [IsActive], [Password], [Role], [FileURL]) VALUES (3, N'furkan', 1, N'furkan', 1, N'C:\Users\fbalt\source\repos\Ppbot2\Ppbot\PpBot\PpBot.UI\Assets\Content\furkan\')
INSERT [dbo].[Users] ([ID], [UserName], [IsActive], [Password], [Role], [FileURL]) VALUES (4, N'essa', 1, N'123123', 1, N'C:\Users\fbalt\source\repos\Ppbot2\Ppbot\PpBot\PpBot.UI\Assets\Content\essa')
SET IDENTITY_INSERT [dbo].[Users] OFF
USE [master]
GO

