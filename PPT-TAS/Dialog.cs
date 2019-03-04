﻿using System;
using System.Windows.Forms;

namespace PPT_TAS {
    public partial class Dialog: Form {
        private Renderer gfx;

        private static readonly int initX = 4, initR = 0;
        private int initY;

        public int desiredX = initX, desiredR = initR;
        public bool desiredHold = false;

        private int[,] board;
        private int[] queue;
        private int current, y, hold;

        public Dialog(int[,] _board, int _current, int _yPos, int _hold, int[] _queue, int _cleared, int _bagIndex) {
            InitializeComponent();

            actions = new Action[7] {
                () => {
                    valueX.Value--;
                },
                () => {
                    valueX.Value++;
                },
                () => {
                    // Hard Drop
                },
                () => {
                    // Soft Drop
                },
                () => {
                    valueR.Value--;
                },
                () => {
                    valueR.Value++;
                },
                () => {
                    valueHold.Checked = !valueHold.Checked;
                }
            };

            board = _board;
            current = _current;
            y = initY = _yPos;
            hold = _hold;
            queue = _queue;

            gfx = new Renderer(ref canvas) {
                board = _board,
                current = _current,
                x = desiredX,
                y = _yPos,
                r = desiredR,
                hold = _hold,
                queue = _queue,
                cleared = _cleared,
                bag = _bagIndex
            };

            gfx.Draw();
        }

        private bool movementCollision(int x) {
            int c = desiredHold? ((hold == 255)? queue[0] : hold) : current;

            foreach ((int, int) offset in Renderer.pieces[c][desiredR]) {
                try {
                    if (board[x - 1 + offset.Item1, 24 - y - offset.Item2] != 255) {
                        return false;
                    }
                } catch {
                    return false;
                }
            }

            return true;
        }

        private void valueX_ValueChanged(object sender, EventArgs e) {
            if (movementCollision((int)valueX.Value)) {
                desiredX = (int)valueX.Value;

                gfx.x = desiredX;
                gfx.Draw();

            } else {
                valueX.Value = desiredX;
            }
        }

        private void valueR_ValueChanged(object sender, EventArgs e) {
            if (valueR.Value == -1) {
                valueR.Value = 3;
                return;
            }

            if (valueR.Value == 4) {
                valueR.Value = 0;
                return;
            }

            if (true /* SRS check? */) {
                desiredR = (int)valueR.Value;

                gfx.r = desiredR;
                gfx.Draw();

            } else {
                valueR.Value = desiredR;
            }
        }

        private void valueHold_CheckedChanged(object sender, EventArgs e) {
            desiredHold = valueHold.Checked;

            valueX.Value = initX;
            y = initY;
            valueR.Value = initR;

            gfx.useHold = desiredHold;
            gfx.Draw();
        }

        private readonly Keys[] keycodes = new Keys[7] {
            Keys.Left,
            Keys.Right,
            Keys.Up,
            Keys.Down,
            Keys.Z,
            Keys.X,
            Keys.Space
        };

        private bool[] keys = new bool[7];
        private Action[] actions;

        private void Dialog_KeyDown(object sender, KeyEventArgs e) {
            for (int i = 0; i < 7; i++) {
                if (e.KeyCode == keycodes[i]) {
                    if (!keys[i]) {
                        actions[i].Invoke();
                    }
                    keys[i] = true;

                    e.Handled = true;
                    return;
                }
            }
        }

        private void Dialog_KeyUp(object sender, KeyEventArgs e) {
            for (int i = 0; i < 7; i++) {
                if (e.KeyCode == keycodes[i]) {
                    keys[i] = false;

                    e.Handled = true;
                    return;
                }
            }
        }
    }
}
