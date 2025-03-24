namespace ExternalTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnemyPattern instance = new EnemyPattern();
            instance.ShowDialog();
        }

        private void buttonWave_Click(object sender, EventArgs e)
        {
            WaveFormation instance = new WaveFormation();
            instance.ShowDialog();
        }
    }
}
