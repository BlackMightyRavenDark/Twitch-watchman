using System;
using System.Windows.Forms;

namespace Twitch_watchman
{
    public partial class FormItemEditor : Form
    {
        public StreamItem StreamItem { get; private set; }
        private readonly bool _editingMode;

        public FormItemEditor(StreamItem streamItem)
        {
            InitializeComponent();

            StreamItem = streamItem;
            if (streamItem != null)
            {
                textBoxChannelName.Text = streamItem.ChannelName;
                textBoxChannelName.Enabled = false;
                checkBoxImportant.Checked = streamItem.IsImportant;
                Text = "Настройки канала";
                _editingMode = true;
            }
            else
            {
                Shown += (s, e) =>
                {
                    textBoxChannelName.Focus();
                };
                _editingMode = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!_editingMode)
            {
                string channelName = textBoxChannelName.Text;

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
