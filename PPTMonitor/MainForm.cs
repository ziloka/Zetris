﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPTMonitor {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private enum MatchType {
            PuzzleLeague,
            FreePlay
        }

        private readonly string[] Regions = new string[183] {
            "Not Set",
            "Japan (Hokkaido)",
            "Japan (Aomori)",
            "Japan (Iwate)",
            "Japan (Miyagi)",
            "Japan (Akita)",
            "Japan (Yamagata)",
            "Japan (Fukushima)",
            "Japan (Ibaraki)",
            "Japan (Tochigi)",
            "Japan (Gunma)",
            "Japan (Saitama)",
            "Japan (Chiba)",
            "Japan (Tokyo)",
            "Japan (Kanagawa)",
            "Japan (Niigata)",
            "Japan (Toyama)",
            "Japan (Ishikawa)",
            "Japan (Fukui)",
            "Japan (Yamanashi)",
            "Japan (Nagano)",
            "Japan (Gifu)",
            "Japan (Shizuoka)",
            "Japan (Aichi)",
            "Japan (Mie)",
            "Japan (Shiga)",
            "Japan (Kyoto)",
            "Japan (Osaka)",
            "Japan (Hyogo)",
            "Japan (Nara)",
            "Japan (Wakayama)",
            "Japan (Tottori)",
            "Japan (Shimane)",
            "Japan (Okayama)",
            "Japan (Hiroshima)",
            "Japan (Yamaguchi)",
            "Japan (Tokushima)",
            "Japan (Kagawa)",
            "Japan (Ehime)",
            "Japan (Kochi)",
            "Japan (Fukuoka)",
            "Japan (Saga)",
            "Japan (Nagasaki)",
            "Japan (Kumamoto)",
            "Japan (Oita)",
            "Japan (Miyazaki)",
            "Japan (Kagoshima)",
            "Japan (Okinawa)",
            "USA (Alabama)",
            "USA (Alaska)",
            "USA (Arizona)",
            "USA (Arkansas)",
            "USA (California)",
            "USA (Colorado)",
            "USA (Connecticut)",
            "USA (Delaware)",
            "USA (Florida)",
            "USA (Georgia)",
            "USA (Hawaii)",
            "USA (Idaho)",
            "USA (Illinois)",
            "USA (Indiana)",
            "USA (Iowa)",
            "USA (Kansas)",
            "USA (Kentucky)",
            "USA (Louisiana)",
            "USA (Maine)",
            "USA (Maryland)",
            "USA (Massachusetts)",
            "USA (Michigan)",
            "USA (Minnesota)",
            "USA (Mississippi)",
            "USA (Missouri)",
            "USA (Montana)",
            "USA (Nebraska)",
            "USA (Nevada)",
            "USA (New Hampshire)",
            "USA (New Jersey)",
            "USA (New Mexico)",
            "USA (New York)",
            "USA (North Carolina)",
            "USA (North Dakota)",
            "USA (Ohio)",
            "USA (Oklahoma)",
            "USA (Oregon)",
            "USA (Pennsylvania)",
            "USA (Rhode Island)",
            "USA (South Carolina)",
            "USA (South Dakota)",
            "USA (Tennessee)",
            "USA (Texas)",
            "USA (Utah)",
            "USA (Vermont)",
            "USA (Virginia)",
            "USA (Washington)",
            "USA (West Virginia)",
            "USA (Wisconsin)",
            "USA (Wyoming)",
            "USA (Other)",
            "Canada",
            "North America (Other)",
            "Austria",
            "Belgium",
            "Bulgaria",
            "Croatia",
            "Czech Republic",
            "Denmark",
            "Spain",
            "Finland",
            "France",
            "Great Britain",
            "Germany",
            "Greece",
            "Hungary",
            "Ireland",
            "Israel",
            "Italy",
            "Netherlands",
            "Norway",
            "Poland",
            "Portugal",
            "Romania",
            "Russian Federation",
            "Slovenia",
            "Switzerland",
            "Slovakia",
            "Sweden",
            "Turkey",
            "Ukraine",
            "Europe (Other)",
            "Argentina",
            "Bahamas",
            "Bolivia",
            "Brazil",
            "Chile",
            "Colombia",
            "Costa Rica",
            "Cuba",
            "Ecuador",
            "Honduras",
            "Jamaica",
            "Mexico",
            "Paraguay",
            "Peru",
            "Trinidad and Tobago",
            "Uruguay",
            "Latin America (Other)",
            "Algeria",
            "Angola",
            "Côte d'Ivoire",
            "Egypt",
            "Ethiopia",
            "Gambia",
            "Ghana",
            "Guinea",
            "Kenya",
            "Morocco",
            "Nigeria",
            "South Africa",
            "Senegal",
            "Togo",
            "Tunisia",
            "Africa (Other)",
            "Australia",
            "Fiji",
            "New Zealand",
            "Oceania (Other)",
            "People's Republic of China",
            "Hong Kong, China",
            "Indonesia",
            "India",
            "Islamic Republic of Iran",
            "Republic of Korea",
            "Saudi Arabia",
            "Malaysia",
            "Pakistan",
            "Philippines",
            "Singapore",
            "Thailand",
            "United Arab Emirates",
            "Uzbekistan",
            "Taiwan, Republic of China",
            "Asia (Other)"
        };

        private readonly string[] Leagues = new string[15] {
            "Student",
            "Beginner",
            "Rookie",
            "Amateur",
            "Ace",
            "Wizard",
            "Professional",
            "Elite",
            "Virtuoso",
            "Star",
            "Superstar",
            "Legend",
            "Golden",
            "Platinum",
            "Grand Master"
        };

        private static string RatioType(int ratio) {
            if (ratio < 34) {
                return "T";
            } else if (ratio > 66) {
                return "P";
            }
            return "X";
        }

        private struct Player {
            public string name;
            public int rating;
            public int league;
            public int playstyle;
            public int region;
            public int regional;
            public int worldwide;
            public int id;
            public string steam;
            public int pref;
            public int character;
            public int gamemode;
            public int voice;
        }

        private Color tetromino(int x) {
            switch (x) {
                case 0:
                    return Color.FromArgb(0, 255, 0);

                case 1:
                    return Color.FromArgb(255, 0, 0);

                case 2:
                    return Color.FromArgb(0, 0, 255);

                case 3:
                    return Color.FromArgb(255, 63, 0);

                case 4:
                    return Color.FromArgb(63, 0, 255);

                case 5:
                    return Color.FromArgb(255, 255, 0);

                case 6:
                    return Color.FromArgb(0, 255, 255);

                case 7:
                    return Color.FromArgb(239, 206, 26);

                case 9:
                    return Color.FromArgb(255, 255, 255);

                case -1:
                case -2:
                    return Color.Transparent;
            }

            return Color.Transparent;
        }

        static VAMemory PPT = new VAMemory("puyopuyotetris");

        static int currentRating, numplayers;
        static Player[] players = new Player[2];
        static int[] score = new int[2] {0, 0};
        static int maxscore = 2;
        static int[,,] board = new int[2, 10, 40];

        private void updateUI() {
            valueScore1.Text = score[0].ToString();
            valueScore2.Text = score[1].ToString();

            valueP1Rating.Text = players[0].rating.ToString();
            valueP2Rating.Text = players[1].rating.ToString();

            valueP1Name.Text = players[0].name;
            valueP2Name.Text = players[1].name;

            valueP1League.Text = Leagues[players[0].league];
            valueP2League.Text = Leagues[players[1].league];

            valueP1Ratio.Text = RatioType(players[0].playstyle);
            valueP2Ratio.Text = RatioType(players[1].playstyle);

            valueP1Region.Text = Regions[players[0].region];
            valueP2Region.Text = Regions[players[1].region];

            valueP1Regional.Text = players[0].regional.ToString();
            valueP2Regional.Text = players[1].regional.ToString();

            valueP1Worldwide.Text = players[0].worldwide.ToString();
            valueP2Worldwide.Text = players[1].worldwide.ToString();

            valueP1CharacterPref.BackgroundImage = (Image)(PPTMonitor.Properties.Resources.ResourceManager.GetObject($"character_{players[0].pref}"));
            valueP2CharacterPref.BackgroundImage = (Image)(PPTMonitor.Properties.Resources.ResourceManager.GetObject($"character_{players[1].pref}"));
            if (players[0].pref == -1) {
                valueP1CharacterPref.BackgroundImage = null;
            }
            if (players[1].pref == -1) {
                valueP2CharacterPref.BackgroundImage = null;
            }

            valueP1Character.BackgroundImage = (Image)(PPTMonitor.Properties.Resources.ResourceManager.GetObject($"character_{players[0].character}"));
            valueP2Character.BackgroundImage = (Image)(PPTMonitor.Properties.Resources.ResourceManager.GetObject($"character_{players[1].character}"));
            if (players[0].character == -1) {
                valueP1Character.BackgroundImage = null;
            }
            if (players[1].character == -1) {
                valueP2Character.BackgroundImage = null;
            }

            valueP1Gamemode.BackgroundImage = (Image)(PPTMonitor.Properties.Resources.ResourceManager.GetObject($"gamemode_{players[0].gamemode}"));
            valueP2Gamemode.BackgroundImage = (Image)(PPTMonitor.Properties.Resources.ResourceManager.GetObject($"gamemode_{players[1].gamemode}"));
            if (players[0].gamemode == -1) {
                valueP1Gamemode.BackgroundImage = null;
            }
            if (players[1].gamemode == -1) {
                valueP2Gamemode.BackgroundImage = null;
            }

            if (players[0].voice == 1) {
                valueP1Voice.BackgroundImage = PPTMonitor.Properties.Resources.voice;
            } else {
                valueP1Voice.BackgroundImage = null;
            }
            if (players[1].voice == 1) {
                valueP2Voice.BackgroundImage = PPTMonitor.Properties.Resources.voice;
            } else {
                valueP2Voice.BackgroundImage = null;
            }

            valuePlayers.Text = numplayers.ToString();
            valueRating.Text = currentRating.ToString();

            board1.Image = new Bitmap(board1.Width, board1.Height);
            using (Graphics gfx = Graphics.FromImage(board1.Image)) {
                for (int i = 0; i < 10; i++) {
                    for (int j = 0; j < 40; j++) {
                        gfx.FillRectangle(new SolidBrush(tetromino(board[0, i, j])), i * (board1.Width / 10), (39 - j) * (board1.Height / 40), board1.Width / 10, board1.Height / 40);
                    }
                }

                gfx.DrawLine(new Pen(Color.Red), 0, board1.Height / 2, board1.Width, board1.Height / 2);
                gfx.Flush();
            }

            board2.Image = new Bitmap(board2.Width, board2.Height);
            using (Graphics gfx = Graphics.FromImage(board2.Image)) {
                for (int i = 0; i < 10; i++) {
                    for (int j = 0; j < 40; j++) {
                        gfx.FillRectangle(new SolidBrush(tetromino(board[1, i, j])), i * (board2.Width / 10), (39 - j) * (board2.Height / 40), board2.Width / 10, board2.Height / 40);
                    }
                }

                gfx.DrawLine(new Pen(Color.Red), 0, board2.Height / 2, board2.Width, board2.Height / 2);
                gfx.Flush();
            }
        }

        private void ScanTimer_Tick(object sender, EventArgs e) {
            int scoreAddress = PPT.ReadInt32(new IntPtr(0x14057F048));

            scoreAddress += 0x38;
                
            maxscore = PPT.ReadInt32(new IntPtr(scoreAddress + 0x10));

            for (int i = 0; i < 2; i++)
                score[i] = PPT.ReadInt32(new IntPtr(scoreAddress + i * 4));

            int playerAddress = PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(0x140473760)) + 0x20)) + 0xD8;
            int leagueAddress = PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(0x140473760)) + 0x68)) + 0x20)) + 0x970)) - 0x38;

            numplayers = PPT.ReadInt16(new IntPtr(playerAddress) - 0x24);

            for (int i = 0; i < 2; i++) {
                players[i].name = PPT.ReadStringUnicode(new IntPtr(playerAddress) + i * 0x50, 0x20);
                players[i].rating = PPT.ReadInt16(new IntPtr(playerAddress) + 0x30 + i * 0x50);

                short temp = PPT.ReadInt16(new IntPtr(leagueAddress) + i * 0x140);

                if (temp != 0)
                    players[i].league = temp - 1;
                
                players[i].playstyle = PPT.ReadInt16(new IntPtr(playerAddress) + 0x32 + i * 0x50);
                players[i].region = PPT.ReadByte(new IntPtr(leagueAddress) - 0x0C + i * 0x140);

                players[i].regional = PPT.ReadInt32(new IntPtr(playerAddress) + 0x28 + i * 0x50);
                players[i].worldwide = PPT.ReadInt32(new IntPtr(playerAddress) + 0x2C + i * 0x50);

                players[i].id = PPT.ReadInt32(new IntPtr(playerAddress) + 0x40 + i * 0x50);

                players[i].steam = $"https://steamcommunity.com/profiles/{(76561197960265728 + players[i].id).ToString()}";
            }

            int prefAddress = PPT.ReadInt32(new IntPtr(
                PPT.ReadInt32(new IntPtr(
                    PPT.ReadInt32(new IntPtr(
                        PPT.ReadInt32(new IntPtr(
                            PPT.ReadInt32(new IntPtr(
                                PPT.ReadInt32(new IntPtr(
                                    PPT.ReadInt32(new IntPtr(
                                        PPT.ReadInt32(new IntPtr(
                                            PPT.ReadInt32(new IntPtr(
                                                PPT.ReadInt32(new IntPtr(
                                                    0x140573A78
                                                )) + 0x20
                                            )) + 0x20
                                        )) + 0x20
                                    )) + 0xA8
                                )) + 0x68
                            )) + 0x90
                        )) + 0x28
                    )) + 0x18
                )) + 0x08
            ));

            players[0].pref = PPT.ReadByte(new IntPtr(prefAddress - 0x9484));
            players[1].pref = PPT.ReadByte(new IntPtr(prefAddress + 0xD4));

            int charAddress = PPT.ReadInt32(new IntPtr(0x140460690));

            for (int i = 0; i < 2; i++) {
                players[i].character = PPT.ReadByte(new IntPtr(charAddress + 0x1c8 + i * 0x30));
                players[i].gamemode = PPT.ReadByte(new IntPtr(charAddress + 0x980 + i * 0x28));
                players[i].voice = (PPT.ReadByte(new IntPtr(charAddress + 0x594)) & (i + 1)) >> i;
            }

            for (int i = numplayers; i < 2; i++) {
                players[i] = new Player();
                players[i].pref = -1;
                players[i].character = -1;
            }

            currentRating = PPT.ReadInt16(new IntPtr(0x140599FF0));

            int boardBase = PPT.ReadInt32(new IntPtr(0x1405989D0));

            int[] boardAddress = new int[] {
                PPT.ReadInt32(new IntPtr(
                    PPT.ReadInt32(new IntPtr(
                        boardBase + 0x3C0
                    )) + 0x18
                )),
                PPT.ReadInt32(new IntPtr(
                    PPT.ReadInt32(new IntPtr(
                        PPT.ReadInt32(new IntPtr(
                            boardBase + 0x30
                        )) + 0x20
                    )) + 0x3C0
                )) + 0x50
            };

            for (int p = 0; p < 2; p++) {
                for (int i = 0; i < 10; i++) {
                    int columnAddress = PPT.ReadInt32(new IntPtr(boardAddress[p] + i * 0x08));
                    for (int j = 0; j < 40; j++) {
                        board[p, i, j] = PPT.ReadInt32(new IntPtr(columnAddress + j * 0x04));
                    }
                }
            }

            updateUI();
        }

        private void writeMatch(MatchType type, int players, int bestof, List<int> matchLog, int ratingChange, Player[] matchPlayers) {
            // TODO: Log Match
        }

        private void valueP1Name_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(players[0].steam);
        }

        private void valueP2Name_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(players[1].steam);
        }

        private void MainForm_Load(object sender, EventArgs e) {
            ScanTimer_Tick(null, EventArgs.Empty);
            // Reset Puzzle
        }

        private void buttonRehook_Click(object sender, EventArgs e) {
            PPT = new VAMemory("puyopuyotetris");
        }
    }
}