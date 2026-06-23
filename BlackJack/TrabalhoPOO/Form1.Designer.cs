namespace TrabalhoPOO
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnComprar = new System.Windows.Forms.Button();
            this.btnParar = new System.Windows.Forms.Button();
            this.btnReiniciar = new System.Windows.Forms.Button();
            this.pnlBanca = new System.Windows.Forms.Panel();
            this.pnlJogador = new System.Windows.Forms.Panel();
            this.lblPontosBanca = new System.Windows.Forms.Label();
            this.lblPontosJogador = new System.Windows.Forms.Label();
            this.lblResultado = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnComprar
            // 
            this.btnComprar.BackColor = System.Drawing.Color.Lime;
            this.btnComprar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComprar.Font = new System.Drawing.Font("Cooper Black", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComprar.Location = new System.Drawing.Point(150, 380);
            this.btnComprar.Name = "btnComprar";
            this.btnComprar.Size = new System.Drawing.Size(120, 35);
            this.btnComprar.TabIndex = 5;
            this.btnComprar.Text = "Comprar";
            this.btnComprar.UseVisualStyleBackColor = false;
            this.btnComprar.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnParar
            // 
            this.btnParar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnParar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnParar.Font = new System.Drawing.Font("Cooper Black", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParar.Location = new System.Drawing.Point(330, 380);
            this.btnParar.Name = "btnParar";
            this.btnParar.Size = new System.Drawing.Size(120, 35);
            this.btnParar.TabIndex = 6;
            this.btnParar.Text = "Parar";
            this.btnParar.UseVisualStyleBackColor = false;
            this.btnParar.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnReiniciar
            // 
            this.btnReiniciar.BackColor = System.Drawing.Color.Cyan;
            this.btnReiniciar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReiniciar.Font = new System.Drawing.Font("Cooper Black", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReiniciar.Location = new System.Drawing.Point(510, 380);
            this.btnReiniciar.Name = "btnReiniciar";
            this.btnReiniciar.Size = new System.Drawing.Size(120, 35);
            this.btnReiniciar.TabIndex = 7;
            this.btnReiniciar.Text = "Reiniciar";
            this.btnReiniciar.UseVisualStyleBackColor = false;
            this.btnReiniciar.Click += new System.EventHandler(this.button3_Click);
            // 
            // pnlBanca
            // 
            this.pnlBanca.BackColor = System.Drawing.Color.Transparent;
            this.pnlBanca.Location = new System.Drawing.Point(12, 40);
            this.pnlBanca.Name = "pnlBanca";
            this.pnlBanca.Size = new System.Drawing.Size(760, 130);
            this.pnlBanca.TabIndex = 0;
            // 
            // pnlJogador
            // 
            this.pnlJogador.BackColor = System.Drawing.Color.Transparent;
            this.pnlJogador.Location = new System.Drawing.Point(12, 230);
            this.pnlJogador.Name = "pnlJogador";
            this.pnlJogador.Size = new System.Drawing.Size(760, 129);
            this.pnlJogador.TabIndex = 1;
            // 
            // lblPontosBanca
            // 
            this.lblPontosBanca.BackColor = System.Drawing.Color.Transparent;
            this.lblPontosBanca.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPontosBanca.ForeColor = System.Drawing.Color.White;
            this.lblPontosBanca.Location = new System.Drawing.Point(12, 15);
            this.lblPontosBanca.Name = "lblPontosBanca";
            this.lblPontosBanca.Size = new System.Drawing.Size(200, 20);
            this.lblPontosBanca.TabIndex = 2;
            this.lblPontosBanca.Text = "Banca: ?";
            // 
            // lblPontosJogador
            // 
            this.lblPontosJogador.BackColor = System.Drawing.Color.Transparent;
            this.lblPontosJogador.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPontosJogador.ForeColor = System.Drawing.Color.White;
            this.lblPontosJogador.Location = new System.Drawing.Point(12, 205);
            this.lblPontosJogador.Name = "lblPontosJogador";
            this.lblPontosJogador.Size = new System.Drawing.Size(200, 20);
            this.lblPontosJogador.TabIndex = 3;
            this.lblPontosJogador.Text = "Jogador: 0 pts";
            // 
            // lblResultado
            // 
            this.lblResultado.BackColor = System.Drawing.Color.Transparent;
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblResultado.ForeColor = System.Drawing.Color.Blue;
            this.lblResultado.Location = new System.Drawing.Point(12, 175);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(760, 30);
            this.lblResultado.TabIndex = 4;
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(100)))), ((int)(((byte)(34)))));
            this.BackgroundImage = global::TrabalhoPOO.Properties.Resources._360_F_1941926973_bMXtWYh3XEUBKnnbkc0FhNYu9dL8B1W9;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(784, 431);
            this.Controls.Add(this.pnlBanca);
            this.Controls.Add(this.pnlJogador);
            this.Controls.Add(this.lblPontosBanca);
            this.Controls.Add(this.lblPontosJogador);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnComprar);
            this.Controls.Add(this.btnParar);
            this.Controls.Add(this.btnReiniciar);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jogo de 21";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnComprar;
        private System.Windows.Forms.Button btnParar;
        private System.Windows.Forms.Button btnReiniciar;
        private System.Windows.Forms.Panel pnlBanca;
        private System.Windows.Forms.Panel pnlJogador;
        private System.Windows.Forms.Label lblPontosBanca;
        private System.Windows.Forms.Label lblPontosJogador;
        private System.Windows.Forms.Label lblResultado;
    }
}