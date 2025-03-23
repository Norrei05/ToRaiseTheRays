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
    }
}
