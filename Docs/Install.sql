USE TombProspectors;
GO

DROP TABLE IF EXISTS Bosses;
DROP TABLE IF EXISTS Comments;
DROP TABLE IF EXISTS DungeonGlyphs;
DROP TABLE IF EXISTS Loot;
DROP TABLE IF EXISTS RootChalices;
DROP TABLE IF EXISTS UserHistory;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Articles;

CREATE TABLE [dbo].[UserHistory] (
	[Id] INT IDENTITY PRIMARY KEY NOT NULL,
	[UserName] VARCHAR(50) NOT NULL,
	[Target] VARCHAR(50) NOT NULL,
	[Action] VARCHAR(50) NOT NULL,
	[Value] VARCHAR(50) NOT NULL,
	[Created] DATETIME2(0) NOT NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[Users] (
	[Id] INT IDENTITY NOT NULL,
	[UserName] VARCHAR(50) PRIMARY KEY NOT NULL,
	[Password] VARCHAR(50) NOT NULL,
	[Email] NVARCHAR(50) NOT NULL,
	[Role] VARCHAR(10) NOT NULL,
	[Created] DATETIME2(0) NOT NULL,
	[LastLogin] DATETIME2(0) NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[RootChalices] (
	ChaliceId VARCHAR(50) PRIMARY KEY NOT NULL,
	ChaliceName VARCHAR(50) NOT NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[Loot] (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	ItemName NVARCHAR(100) NOT NULL,
	Note NVARCHAR(250),
	WikiLink NVARCHAR(250),
	Updated DATETIME2(0) NOT NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[DungeonGlyphs] (
	Glyph VARCHAR(20) PRIMARY KEY NOT NULL,
	ShortDescription VARCHAR(250) NOT NULL,
	Layers INT NOT NULL,
	RootChalice VARCHAR(50) NOT NULL,
	Fetid BIT NOT NULL DEFAULT(0),
	Rotted BIT NOT NULL DEFAULT(0),
	Cursed BIT NOT NULL DEFAULT(0),
	Sinister BIT NOT NULL DEFAULT(0),
	SaveEdited BIT NOT NULL DEFAULT(0),
	Bosses VARCHAR(250) NOT NULL,
	Loot VARCHAR(250) NOT NULL,
	Notes NVARCHAR(MAX),
	Submitter VARCHAR(50) NOT NULL,
	Upvotes INT NOT NULL DEFAULT(1),
	Downvotes INT NOT NULL DEFAULT(0),
	ClosedVotes INT NOT NULL DEFAULT(0),
	Updated DATETIME2(0) NOT NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[Bosses] (
	Id INT IDENTITY PRIMARY KEY,
	BossName VARCHAR(150) NOT NULL,
	WikiLink NVARCHAR(250),
	Updated DATETIME2(0) NOT NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[Comments] (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	CommentText VARCHAR(500) NOT NULL,
	Glyph VARCHAR(20) NOT NULL,
	PostedBy VARCHAR(50) NOT NULL,
	Posted DATETIME2(0) NOT NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[Articles] (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Section VARCHAR(100) NOT NULL,
	Title VARCHAR(500) NOT NULL,
	Content VARCHAR(MAX) NOT NULL,
	PostedBy VARCHAR(50) NOT NULL,
	Posted DATETIME2(0) NOT NULL
) ON [PRIMARY];

-- Bosses
INSERT INTO Bosses VALUES('Abhorrent Beast',					'http://www.bloodborne-wiki.com/2015/03/abhorrent-beast.html',							GETDATE());
INSERT INTO Bosses VALUES('Amygdala',							'http://www.bloodborne-wiki.com/2015/03/amygdala.html',									GETDATE());
INSERT INTO Bosses VALUES('Beast-Possessed Soul',				'http://www.bloodborne-wiki.com/2015/03/beast-possessed-soul.html',						GETDATE());
INSERT INTO Bosses VALUES('Blood-starved Beast',				'http://www.bloodborne-wiki.com/2015/03/blood-starved-beast.html',						GETDATE());
INSERT INTO Bosses VALUES('Bloodletting Beast',					'http://www.bloodborne-wiki.com/2015/03/bloodletting-beast.html',						GETDATE());
INSERT INTO Bosses VALUES('Bloodletting Beast (Headless)',		'http://www.bloodborne-wiki.com/2015/03/bloodletting-beast.html',						GETDATE());
INSERT INTO Bosses VALUES('Bloodletting Beast (Parasite)',		'http://www.bloodborne-wiki.com/2015/03/bloodletting-beast.html',						GETDATE());
INSERT INTO Bosses VALUES('Brainsucker',						'http://www.bloodborne-wiki.com/2015/04/brainsucker.html',								GETDATE());
INSERT INTO Bosses VALUES('Celestial Emissary',					'http://www.bloodborne-wiki.com/2015/03/celestial-emissary.html',						GETDATE());
INSERT INTO Bosses VALUES('Ebrietas, Daughter of the Cosmos',	'http://www.bloodborne-wiki.com/2015/03/ebrietas-daughter-of-cosmos.html',				GETDATE());
INSERT INTO Bosses VALUES('Forgotten Madman',					'http://www.bloodborne-wiki.com/2015/04/forgotten-madman.html',							GETDATE());
INSERT INTO Bosses VALUES('Forgotten Escort',					'http://www.bloodborne-wiki.com/2015/04/forgotten-madman.html',							GETDATE());
INSERT INTO Bosses VALUES('Keeper of the Old Lords',			'http://www.bloodborne-wiki.com/2015/03/keeper-of-old-lords.html',						GETDATE());
INSERT INTO Bosses VALUES('Loran Darkbeast',					'http://www.bloodborne-wiki.com/2015/02/darkbeast-paarl.html',							GETDATE());
INSERT INTO Bosses VALUES('Loran Silverbeast',					'http://www.bloodborne-wiki.com/2015/06/loran-silverbeast.html',						GETDATE());
INSERT INTO Bosses VALUES('Maneater Boar',						'http://www.bloodborne-wiki.com/2015/03/maneater-boar.html',							GETDATE());
INSERT INTO Bosses VALUES('Merciless Watcher',					'http://www.bloodborne-wiki.com/2015/10/merciless-watchers-watcher-chieftain.html',		GETDATE());
INSERT INTO Bosses VALUES('Pthumerian Descendant',				'http://www.bloodborne-wiki.com/2015/03/pthumerian-descendant.html',					GETDATE());
INSERT INTO Bosses VALUES('Pthumerian Elder',					'http://www.bloodborne-wiki.com/2015/03/pthumerian-elder.html',							GETDATE());
INSERT INTO Bosses VALUES('Undead Giant (Cannon)',				'http://www.bloodborne-wiki.com/2015/08/undead-giant-hatchet-and-cannon.html',			GETDATE());
INSERT INTO Bosses VALUES('Undead Giant (Curved Blades)',		'http://www.bloodborne-wiki.com/2015/03/undead-giant.html',								GETDATE());
INSERT INTO Bosses VALUES('Undead Giant (Hook)',				'http://www.bloodborne-wiki.com/2015/08/undead-giant-hatchet-and-cannon.html',			GETDATE());
INSERT INTO Bosses VALUES('Watchdog of the Old Lords',			'http://www.bloodborne-wiki.com/2015/03/ancient-guard-dog.html',						GETDATE());
INSERT INTO Bosses VALUES('Yharnam, Pthumerian Queen',			'http://www.bloodborne-wiki.com/2015/04/yharnam-pthumerian-queen.html',					GETDATE());

-- RootChalices
INSERT INTO RootChalices VALUES('pthumeruRoot',				'Pthumeru Root Chalice');
INSERT INTO RootChalices VALUES('pthumeruCentral',			'Central Pthumeru Root Chalice');
INSERT INTO RootChalices VALUES('pthumeruLower',			'Lower Pthumeru Root Chalice');
INSERT INTO RootChalices VALUES('pthumeruSinisterLower',	'Sinister Lower Pthumeru Root Chalice');
INSERT INTO RootChalices VALUES('pthumeruCursedDefiled',	'Pthumeru Cursed and Defiled Root Chalice');
INSERT INTO RootChalices VALUES('pthumerIhyll',				'Pthumeru Ihyll Root Chalice');
INSERT INTO RootChalices VALUES('pthumeruSinisterIhyll',	'Sinister Pthumeru Ihyll Root Chalice');
INSERT INTO RootChalices VALUES('hintertombRoot',			'Hintertomb Root Chalice');
INSERT INTO RootChalices VALUES('hintertombLower',			'Lower Hintertomb Root Chalice');
INSERT INTO RootChalices VALUES('hintertombSinister',		'Sinister Hintertomb Root Chalice');
INSERT INTO RootChalices VALUES('ailingRoot',				'Ailing Loran Root Chalice');
INSERT INTO RootChalices VALUES('ailingLower',				'Lower Ailing Loran Root Chalice');
INSERT INTO RootChalices VALUES('ailingSinister',			'Sinister Lower Loran Root Chalice');
INSERT INTO RootChalices VALUES('iszRoot',					'Isz Root Chalice');
INSERT INTO RootChalices VALUES('iszSinister',				'Sinister Isz Root Chalice');

-- Loot
-- Right hand weapons
INSERT INTO Loot VALUES('Amygdalan Arm',				NULL,	'http://www.bloodborne-wiki.com/2015/11/amygdalan-arm.html',			GETDATE());
INSERT INTO Loot VALUES('Lost Amygdalan Arm',			NULL,	'http://www.bloodborne-wiki.com/2015/11/amygdalan-arm.html',			GETDATE());
INSERT INTO Loot VALUES('Uncanny Amygdalan Arm',		NULL,	'http://www.bloodborne-wiki.com/2015/11/amygdalan-arm.html',			GETDATE());
INSERT INTO Loot VALUES('Beast Claw',					NULL,	'http://www.bloodborne-wiki.com/2015/03/beast-claw.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Beast Claw',				NULL,	'http://www.bloodborne-wiki.com/2015/03/beast-claw.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Beast Claw',			NULL,	'http://www.bloodborne-wiki.com/2015/03/beast-claw.html',				GETDATE());
INSERT INTO Loot VALUES('Beasthunter Saif',				NULL,	'http://www.bloodborne-wiki.com/2015/11/beasthunter-saif.html',			GETDATE());
INSERT INTO Loot VALUES('Lost Beasthunter Saif',		NULL,	'http://www.bloodborne-wiki.com/2015/11/beasthunter-saif.html',			GETDATE());
INSERT INTO Loot VALUES('Uncanny Beasthunter Saif',		NULL,	'http://www.bloodborne-wiki.com/2015/11/beasthunter-saif.html',			GETDATE());
INSERT INTO Loot VALUES('Beast Cutter',					NULL,	'http://www.bloodborne-wiki.com/2015/11/beast-cutter.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Beast Cutter',			NULL,	'http://www.bloodborne-wiki.com/2015/11/beast-cutter.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Beast Cutter',			NULL,	'http://www.bloodborne-wiki.com/2015/11/beast-cutter.html',				GETDATE());
INSERT INTO Loot VALUES('Blade of Mercy',				NULL,	'http://www.bloodborne-wiki.com/2015/03/warped-twinblades.html',		GETDATE());
INSERT INTO Loot VALUES('Lost Blade of Mercy',			NULL,	'http://www.bloodborne-wiki.com/2015/03/warped-twinblades.html',		GETDATE());
INSERT INTO Loot VALUES('Uncanny Blade of Mercy',		NULL,	'http://www.bloodborne-wiki.com/2015/03/warped-twinblades.html',		GETDATE());
INSERT INTO Loot VALUES('Bloodletter',					NULL,	'http://www.bloodborne-wiki.com/2015/11/bloodletter.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Bloodletter',				NULL,	'http://www.bloodborne-wiki.com/2015/11/bloodletter.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Bloodletter',			NULL,	'http://www.bloodborne-wiki.com/2015/11/bloodletter.html',				GETDATE());
INSERT INTO Loot VALUES('Boomhammer',					NULL,	'http://www.bloodborne-wiki.com/2015/11/boom-hammer.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Boomhammer',				NULL,	'http://www.bloodborne-wiki.com/2015/11/boom-hammer.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Boomhammer',			NULL,	'http://www.bloodborne-wiki.com/2015/11/boom-hammer.html',				GETDATE());
INSERT INTO Loot VALUES('Burial Blade',					NULL,	'http://www.bloodborne-wiki.com/2015/03/burial-blade.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Burial Blade',			NULL,	'http://www.bloodborne-wiki.com/2015/03/burial-blade.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Burial Blade',			NULL,	'http://www.bloodborne-wiki.com/2015/03/burial-blade.html',				GETDATE());
INSERT INTO Loot VALUES('Chikage',						NULL,	'http://www.bloodborne-wiki.com/2015/03/chikage.html',					GETDATE());
INSERT INTO Loot VALUES('Lost Chikage',					NULL,	'http://www.bloodborne-wiki.com/2015/03/chikage.html',					GETDATE());
INSERT INTO Loot VALUES('Uncanny Chikage',				NULL,	'http://www.bloodborne-wiki.com/2015/03/chikage.html',					GETDATE());
INSERT INTO Loot VALUES('Church Pick',					NULL,	'http://www.bloodborne-wiki.com/2015/11/church-pick.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Church Pick',				NULL,	'http://www.bloodborne-wiki.com/2015/11/church-pick.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Church Pick',			NULL,	'http://www.bloodborne-wiki.com/2015/11/church-pick.html',				GETDATE());
INSERT INTO Loot VALUES('Holy Moonlight Sword',			NULL,	'http://www.bloodborne-wiki.com/2015/11/holy-moonlight-sword.html',		GETDATE());
INSERT INTO Loot VALUES('Lost Holy Moonlight Sword',	NULL,	'http://www.bloodborne-wiki.com/2015/11/holy-moonlight-sword.html',		GETDATE());
INSERT INTO Loot VALUES('Uncanny Holy Moonlight Sword',	NULL,	'http://www.bloodborne-wiki.com/2015/11/holy-moonlight-sword.html',		GETDATE());
INSERT INTO Loot VALUES('Hunter Axe',					NULL,	'http://www.bloodborne-wiki.com/2015/03/hunters-axe.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Hunter Axe',				NULL,	'http://www.bloodborne-wiki.com/2015/03/hunters-axe.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Hunter Axe',			NULL,	'http://www.bloodborne-wiki.com/2015/03/hunters-axe.html',				GETDATE());
INSERT INTO Loot VALUES('Kirkhammer',					NULL,	'http://www.bloodborne-wiki.com/2015/03/kirkhammer.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Kirkhammer',				NULL,	'http://www.bloodborne-wiki.com/2015/03/kirkhammer.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Kirkhammer',			NULL,	'http://www.bloodborne-wiki.com/2015/03/kirkhammer.html',				GETDATE());
INSERT INTO Loot VALUES('Kos Parasite',					NULL,	'http://www.bloodborne-wiki.com/2015/11/kos-parasite.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Kos Parasite',			NULL,	'http://www.bloodborne-wiki.com/2015/11/kos-parasite.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Kos Parasite',			NULL,	'http://www.bloodborne-wiki.com/2015/11/kos-parasite.html',				GETDATE());
INSERT INTO Loot VALUES('Logarius Wheel',				NULL,	'http://www.bloodborne-wiki.com/2015/03/logarius-wheel.html',			GETDATE());
INSERT INTO Loot VALUES('Lost Logarius Wheel',			NULL,	'http://www.bloodborne-wiki.com/2015/03/logarius-wheel.html',			GETDATE());
INSERT INTO Loot VALUES('Uncanny Logarius Wheel',		NULL,	'http://www.bloodborne-wiki.com/2015/03/logarius-wheel.html',			GETDATE());
INSERT INTO Loot VALUES('Ludwigs Holy Blade',			NULL,	'http://www.bloodborne-wiki.com/2015/03/ludwigs-holy-blade.html',		GETDATE());
INSERT INTO Loot VALUES('Lost Ludwigs Holy Blade',		NULL,	'http://www.bloodborne-wiki.com/2015/03/ludwigs-holy-blade.html',		GETDATE());
INSERT INTO Loot VALUES('Uncanny Ludwigs Holy Blade',	NULL,	'http://www.bloodborne-wiki.com/2015/03/ludwigs-holy-blade.html',		GETDATE());
INSERT INTO Loot VALUES('Rakuyo',						NULL,	'http://www.bloodborne-wiki.com/2015/11/rakuyo.html',					GETDATE());
INSERT INTO Loot VALUES('Lost Rakuyo',					NULL,	'http://www.bloodborne-wiki.com/2015/11/rakuyo.html',					GETDATE());
INSERT INTO Loot VALUES('Uncanny Rakuyo',				NULL,	'http://www.bloodborne-wiki.com/2015/11/rakuyo.html',					GETDATE());
INSERT INTO Loot VALUES('Reiterpallasch',				NULL,	'http://www.bloodborne-wiki.com/2015/03/bayonet-reiterpallasch.html',	GETDATE());
INSERT INTO Loot VALUES('Lost Reiterpallasch',			NULL,	'http://www.bloodborne-wiki.com/2015/03/bayonet-reiterpallasch.html',	GETDATE());
INSERT INTO Loot VALUES('Uncanny Reiterpallasch',		NULL,	'http://www.bloodborne-wiki.com/2015/03/bayonet-reiterpallasch.html',	GETDATE());
INSERT INTO Loot VALUES('Rifle Spear',					NULL,	'http://www.bloodborne-wiki.com/2015/03/rifle-spear.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Rifle Spear',				NULL,	'http://www.bloodborne-wiki.com/2015/03/rifle-spear.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Rifle Spear',			NULL,	'http://www.bloodborne-wiki.com/2015/03/rifle-spear.html',				GETDATE());
INSERT INTO Loot VALUES('Saw Spear',					NULL,	'http://www.bloodborne-wiki.com/2015/03/saw-spear.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Saw Spear',				NULL,	'http://www.bloodborne-wiki.com/2015/03/saw-spear.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Saw Spear',			NULL,	'http://www.bloodborne-wiki.com/2015/03/saw-spear.html',				GETDATE());
INSERT INTO Loot VALUES('Saw Cleaver',					NULL,	'http://www.bloodborne-wiki.com/2015/03/saw-cleaver.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Saw Cleaver',				NULL,	'http://www.bloodborne-wiki.com/2015/03/saw-cleaver.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Saw Cleaver',			NULL,	'http://www.bloodborne-wiki.com/2015/03/saw-cleaver.html',				GETDATE());
INSERT INTO Loot VALUES('Simons Bowblade',				NULL,	'http://www.bloodborne-wiki.com/2015/11/simons-bowblade.html',			GETDATE());
INSERT INTO Loot VALUES('Lost Simons Bowblade',			NULL,	'http://www.bloodborne-wiki.com/2015/11/simons-bowblade.html',			GETDATE());
INSERT INTO Loot VALUES('Uncanny Simons Bowblade',		NULL,	'http://www.bloodborne-wiki.com/2015/11/simons-bowblade.html',			GETDATE());
INSERT INTO Loot VALUES('Stake Driver',					NULL,	'http://www.bloodborne-wiki.com/2015/03/stake-driver.html',				GETDATE());
INSERT INTO Loot VALUES('Lost Stake Driver',			NULL,	'http://www.bloodborne-wiki.com/2015/03/stake-driver.html',				GETDATE());
INSERT INTO Loot VALUES('Uncanny Stake Driver',			NULL,	'http://www.bloodborne-wiki.com/2015/03/stake-driver.html',				GETDATE());
INSERT INTO Loot VALUES('Threaded Cane',				NULL,	'http://www.bloodborne-wiki.com/2015/03/threaded-cane.html',			GETDATE());
INSERT INTO Loot VALUES('Lost Threaded Cane',			NULL,	'http://www.bloodborne-wiki.com/2015/03/threaded-cane.html',			GETDATE());
INSERT INTO Loot VALUES('Uncanny Threaded Cane',		NULL,	'http://www.bloodborne-wiki.com/2015/03/threaded-cane.html',			GETDATE());
INSERT INTO Loot VALUES('Tonitrus',						NULL,	'http://www.bloodborne-wiki.com/2015/03/tonitrus.html',					GETDATE());
INSERT INTO Loot VALUES('Lost Tonitrus',				NULL,	'http://www.bloodborne-wiki.com/2015/03/tonitrus.html',					GETDATE());
INSERT INTO Loot VALUES('Uncanny Tonitrus',				NULL,	'http://www.bloodborne-wiki.com/2015/03/tonitrus.html',					GETDATE());
INSERT INTO Loot VALUES('Whirligig Saw',				NULL,	'http://www.bloodborne-wiki.com/2015/11/whirligig-saw.html',			GETDATE());
INSERT INTO Loot VALUES('Lost Whirligig Saw',			NULL,	'http://www.bloodborne-wiki.com/2015/11/whirligig-saw.html',			GETDATE());
INSERT INTO Loot VALUES('Uncanny Whirligig Saw',		NULL,	'http://www.bloodborne-wiki.com/2015/11/whirligig-saw.html',			GETDATE());

-- Left hand weapons
INSERT INTO Loot VALUES('Cannon',						NULL,	'http://www.bloodborne-wiki.com/2015/03/cannon.html',					GETDATE());
INSERT INTO Loot VALUES('Church Cannon',				NULL,	'http://www.bloodborne-wiki.com/2015/11/church-cannon.html',			GETDATE());
INSERT INTO Loot VALUES('Evelyn',						NULL,	'http://www.bloodborne-wiki.com/2015/03/evelyn.html',					GETDATE());
INSERT INTO Loot VALUES('Fist of Gratia',				NULL,	'http://www.bloodborne-wiki.com/2015/11/fist-of-gratia.html',			GETDATE());
INSERT INTO Loot VALUES('FlameSprayer',					NULL,	'http://www.bloodborne-wiki.com/2015/03/flamesprayer.html',				GETDATE());
INSERT INTO Loot VALUES('Gatling Gun',					NULL,	'http://www.bloodborne-wiki.com/2015/11/gatling-gun.html',				GETDATE());
INSERT INTO Loot VALUES('Hunter Blunderbuss',			NULL,	'http://www.bloodborne-wiki.com/2015/03/hunter-blunderbuss.html',		GETDATE());
INSERT INTO Loot VALUES('Hunter Pistol',				NULL,	'http://www.bloodborne-wiki.com/2015/03/hunter-pistol.html',			GETDATE());
INSERT INTO Loot VALUES('Hunters Torch',				NULL,	'http://www.bloodborne-wiki.com/2015/03/hunters-torch.html',			GETDATE());
INSERT INTO Loot VALUES('Loch Shield',					NULL,	'http://www.bloodborne-wiki.com/2015/11/loch-shield.html',				GETDATE());
INSERT INTO Loot VALUES('Ludwigs Rifle',				NULL,	'http://www.bloodborne-wiki.com/2015/03/ludwigs-rifle.html',			GETDATE());
INSERT INTO Loot VALUES('Piercing Rifle',				NULL,	'http://www.bloodborne-wiki.com/2015/11/piercing-rifle.html',			GETDATE());
INSERT INTO Loot VALUES('Repeating Pistol',				NULL,	'http://www.bloodborne-wiki.com/2015/03/repeating-pistol.html',			GETDATE());
INSERT INTO Loot VALUES('Rosmarinus',					NULL,	'http://www.bloodborne-wiki.com/2015/03/rosmarinus.html',				GETDATE());
INSERT INTO Loot VALUES('Torch',						NULL,	'http://www.bloodborne-wiki.com/2015/03/torch.html',					GETDATE());
INSERT INTO Loot VALUES('Wooden Shield',				NULL,	'http://www.bloodborne-wiki.com/2015/03/wooden-shield.html',			GETDATE());

-- Hunter Tools
INSERT INTO Loot VALUES('A Call Beyond',				NULL,	'http://www.bloodborne-wiki.com/2015/03/a-call-beyond.html',			GETDATE());
INSERT INTO Loot VALUES('Accursed Brew',				NULL,	'http://www.bloodborne-wiki.com/2015/11/accursed-brew.html',			GETDATE());
INSERT INTO Loot VALUES('Augur of Ebrietas',			NULL,	'http://www.bloodborne-wiki.com/2015/03/augur-of-ebrietas.html',		GETDATE());
INSERT INTO Loot VALUES('Beast Roar',					NULL,	'http://www.bloodborne-wiki.com/2015/03/beast-roar.html',				GETDATE());
INSERT INTO Loot VALUES('Blacksky Eye',					NULL,	'http://www.bloodborne-wiki.com/2015/11/blacksky-eye.html',				GETDATE());
INSERT INTO Loot VALUES('Choir Bell',					NULL,	'http://www.bloodborne-wiki.com/2015/03/choir-bell.html',				GETDATE());
INSERT INTO Loot VALUES('Empty Phantasm Shell',			NULL,	'http://www.bloodborne-wiki.com/2015/03/empty-phantasm-shell.html',		GETDATE());
INSERT INTO Loot VALUES('Executioners Gloves',			NULL,	'http://www.bloodborne-wiki.com/2015/03/executioners-gloves.html',		GETDATE());
INSERT INTO Loot VALUES('Madaras Whistle',				NULL,	'http://www.bloodborne-wiki.com/2015/11/madaras-whistle.html',			GETDATE());
INSERT INTO Loot VALUES('Messengers Gift',				NULL,	'http://www.bloodborne-wiki.com/2015/03/messengers-gift.html',			GETDATE());
INSERT INTO Loot VALUES('Old Hunter Bone',				NULL,	'http://www.bloodborne-wiki.com/2015/03/old-hunter-bone.html',			GETDATE());
INSERT INTO Loot VALUES('Tiny Tonitrus',				NULL,	'http://www.bloodborne-wiki.com/2015/03/tiny-tonitrus.html',			GETDATE());

-- Misc
INSERT INTO Loot VALUES('Chalice Bath Messengers',		NULL,	'http://www.bloodborne-wiki.com/2015/04/chalice-bath-messengers.html',	GETDATE());
INSERT INTO Loot VALUES('Oddity',						NULL,	NULL,																	GETDATE());