using System;
using System.Windows.Forms;

public class focusTextBox : TextBox
{
    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        this.BeginInvoke(new MethodInvoker(SelectText));
    }
    private void SelectText()
    {
        this.SelectionStart = 0;
        this.SelectionLength = this.Text.Length;
    }
}