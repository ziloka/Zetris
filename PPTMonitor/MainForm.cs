﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private enum MatchState {
            Menu,
            Match,
            Finished
        }

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
        }

        static VAMemory PPT = new VAMemory("puyopuyotetris");

        static int startingRating, currentRating, wins = 0, losses = 0, numplayers;

        static MatchState match = MatchState.Finished;
        static int[] score = new int[4] {0, 0, 0, 0};
        static int[] sets = new int[4] {0, 0, 0, 0};
        static int[] total = new int[4] {0, 0, 0, 0};
       
        static int maxscore = 2;
        static List<int> history = new List<int>();

        static Player[] players = new Player[4];

        private void updateUI() {
            valueScore1.Text = score[0].ToString();
            valueScore2.Text = score[1].ToString();
            valueScore3.Text = score[2].ToString();
            valueScore4.Text = score[3].ToString();

            valueSets1.Text = sets[0].ToString();
            valueSets2.Text = sets[1].ToString();
            valueSets3.Text = sets[2].ToString();
            valueSets4.Text = sets[3].ToString();

            valueTotal1.Text = total[0].ToString();
            valueTotal2.Text = total[1].ToString();
            valueTotal3.Text = total[2].ToString();
            valueTotal4.Text = total[3].ToString();

            valueP1Rating.Text = players[0].rating.ToString();
            valueP2Rating.Text = players[1].rating.ToString();
            valueP3Rating.Text = players[2].rating.ToString();
            valueP4Rating.Text = players[3].rating.ToString();

            valueP1Name.Text = players[0].name;
            valueP2Name.Text = players[1].name;
            valueP3Name.Text = players[2].name;
            valueP4Name.Text = players[3].name;

            valueP1League.Text = Leagues[players[0].league];
            valueP2League.Text = Leagues[players[1].league];
            valueP3League.Text = Leagues[players[2].league];
            valueP4League.Text = Leagues[players[3].league];

            valueP1Ratio.Text = RatioType(players[0].playstyle);
            valueP2Ratio.Text = RatioType(players[1].playstyle);
            valueP3Ratio.Text = RatioType(players[2].playstyle);
            valueP4Ratio.Text = RatioType(players[3].playstyle);

            valueP1Region.Text = Regions[players[0].region];
            valueP2Region.Text = Regions[players[1].region];
            valueP3Region.Text = Regions[players[2].region];
            valueP4Region.Text = Regions[players[3].region];

            valueP1Regional.Text = players[0].regional.ToString();
            valueP2Regional.Text = players[1].regional.ToString();
            valueP3Regional.Text = players[2].regional.ToString();
            valueP4Regional.Text = players[3].regional.ToString();
            
            valueP1Worldwide.Text = players[0].worldwide.ToString();
            valueP2Worldwide.Text = players[1].worldwide.ToString();
            valueP3Worldwide.Text = players[2].worldwide.ToString();
            valueP4Worldwide.Text = players[3].worldwide.ToString();

            valuePlayers.Text = numplayers.ToString();

            valueWins.Text = wins.ToString();
            valueLosses.Text = losses.ToString();
            
            valueCurrentRating.Text = currentRating.ToString();
            valueRatingDifference.Text = (currentRating - startingRating).ToString();
        }

        private void ScanTimer_Tick(object sender, EventArgs e) {
            int temp;

            int scoreAddress = PPT.ReadInt32(new IntPtr(0x14057F048));
            valueScoreAddress.Text = "0x" + scoreAddress.ToString("X8");

            if (scoreAddress == 0x0) {
                if (match == MatchState.Finished) {
                    match = MatchState.Menu;

                    buttonResetBattle_Click(sender, e);
                }
            } else {
                if (match == MatchState.Menu)
                    match = MatchState.Match;

                scoreAddress += 0x38;
            }

            maxscore = PPT.ReadInt32(new IntPtr(scoreAddress + 0x10));

            for (int i = 0; i < 4; i++) {
                temp = PPT.ReadInt32(new IntPtr(scoreAddress) + i * 4);
                if (temp > score[i]) {
                    total[i]++;
                    history.Add(i);
                    if (temp >= maxscore) {
                        sets[i]++;
                    }
                }
                score[i] = temp;
            }

            int playerAddress = PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(0x140473760)) + 0x20)) + 0xD8;
            int leagueAddress = PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(PPT.ReadInt32(new IntPtr(0x140473760)) + 0x68)) + 0x20)) + 0x970)) - 0x38;

            valuePlayerAddress.Text = "0x" + playerAddress.ToString("X8");
            valueLeagueAddress.Text = "0x" + leagueAddress.ToString("X8");

            numplayers = PPT.ReadInt16(new IntPtr(playerAddress) - 0x24);

            for (int i = 0; i < 4; i++) {
                players[i].name = PPT.ReadStringUnicode(new IntPtr(playerAddress) + i * 0x50, 0x20);
                players[i].rating = PPT.ReadInt16(new IntPtr(playerAddress) + 0x30 + i * 0x50);

                temp = PPT.ReadInt16(new IntPtr(leagueAddress) + i * 0x140);

                if (temp != 0)
                    players[i].league = temp - 1;
                
                players[i].playstyle = PPT.ReadInt16(new IntPtr(playerAddress) + 0x32 + i * 0x50);
                players[i].region = PPT.ReadByte(new IntPtr(leagueAddress) - 0xC + i * 0x140);

                players[i].regional = PPT.ReadInt32(new IntPtr(playerAddress) + 0x28 + i * 0x50);
                players[i].worldwide = PPT.ReadInt32(new IntPtr(playerAddress) + 0x2C + i * 0x50);

                players[i].id = PPT.ReadInt32(new IntPtr(playerAddress) + 0x40 + i * 0x50);
            }

            for (int i = numplayers; i < 4; i++) {
                players[i] = new Player();
            }

            temp = PPT.ReadInt16(new IntPtr(0x140599FF0));
            if (temp != currentRating) {
                if (match == MatchState.Match) {
                    writeMatch(MatchType.PuzzleLeague, numplayers, maxscore, history, temp - currentRating, players);
                    match = MatchState.Finished;
                }

                if (temp > currentRating) {
                    wins++;
                } else if (temp < currentRating) {
                    losses++;
                }
            }
            currentRating = temp;

            temp = PPT.ReadInt32(new IntPtr(0x140573A78));
            if (temp != 0x0) {
                if (match == MatchState.Match) {
                    writeMatch(MatchType.FreePlay, numplayers, maxscore, history, 0, players);
                    match = MatchState.Finished;
                }
            }

            updateUI();
        }

        private void writeMatch(MatchType type, int players, int bestof, List<int> matchLog, int ratingChange, Player[] matchPlayers) {
            log.Text += Environment.NewLine + $"writeMatch called {type}, {players}, {bestof}, {{{string.Join(", ", matchLog.ToArray())}}}, {ratingChange}, {matchPlayers[1].name}";
        }
        
        private void buttonResetBattle_Click(object sender, EventArgs e) {
            for (int i = 0; i < 4; i++) {
                score[i] = 0;
                sets[i] = 0;
                total[i] = 0;
            }

            history.Clear();

            valueScore1.Text = score[0].ToString();
            valueScore2.Text = score[1].ToString();
            valueScore3.Text = score[2].ToString();
            valueScore4.Text = score[3].ToString();

            valueSets1.Text = sets[0].ToString();
            valueSets2.Text = sets[1].ToString();
            valueSets3.Text = sets[2].ToString();
            valueSets4.Text = sets[3].ToString();

            valueTotal1.Text = total[0].ToString();
            valueTotal2.Text = total[1].ToString();
            valueTotal3.Text = total[2].ToString();
            valueTotal4.Text = total[3].ToString();
        }

        private void buttonResetPuzzle_Click(object sender, EventArgs e) {
            startingRating = currentRating;

            valueStartingRating.Text = startingRating.ToString();
            valueCurrentRating.Text = currentRating.ToString();
            valueRatingDifference.Text = (currentRating - startingRating).ToString();

            wins = losses = 0;
            valueWins.Text = wins.ToString();
            valueLosses.Text = losses.ToString();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            ScanTimer_Tick(null, EventArgs.Empty);
            buttonResetPuzzle_Click(null, EventArgs.Empty);
        }

        private void buttonRehook_Click(object sender, EventArgs e) {
            PPT = new VAMemory("puyopuyotetris");
        }
    }
}