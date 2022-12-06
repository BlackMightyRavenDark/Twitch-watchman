using System;
using System.Windows.Forms;

namespace Twitch_watchman
{
    public partial class FormItemEditor : Form
    {
        public StreamItem StreamItem { get; private set; }
        private readonly bool EditingMode;

        public FormItemEditor(StreamItem streamItem)
        {
            InitializeComponent();

            StreamItem = streamItem;
            if (streamItem != null)
            {
                textBoxChannelName.Text = streamItem.ChannelName;
                textBoxChannelName.Enabled = false;
                numericUpDownCopiesCount.Value = streamItem.CopiesCount;
                checkBoxImportant.Checked = streamItem.IsImportant;
                Text = "Настройки канала";
                EditingMode = true;
            }
            else
            {
                Shown += (s, e) =>
                {
                    textBoxChannelName.Focus();
                };
                EditingMode = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string channelName = textBoxChannelName.Text;
            if (!EditingMode)
            {
                if (string.IsNullOrEmpty(channelName) || string.IsNullOrWhiteSpace(channelName))
                {
                    MessageBox.Show("Введите название канала!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (channelName.Contains(" "))
                {
                    MessageBox.Show("Название канала не должно содержать пробелов!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Utils.channelNames.Contains(channelName.ToLower()))
                {
                    MessageBox.Show("Такой канал уже есть в списке!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                StreamItem = new StreamItem(channelName);
            }
            StreamItem.CopiesCount = (int)numericUpDownCopiesCount.Value;
            StreamItem.IsImportant = checkBoxImportant.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
