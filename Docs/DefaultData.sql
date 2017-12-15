use TombProspectors;
go

-- Reset all
TRUNCATE TABLE Bosses;
TRUNCATE TABLE RootChalices;

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

-- Standard loot TODO