using Avalonia.Controls;

namespace Admission_of_Applicants.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Автоматически фиксировать любые изменения ячеек при потере фокуса или переключениях
        var dataGrid = this.FindControl<DataGrid>("MyDataGrid");
        if (dataGrid != null)
        {
            dataGrid.CellEditEnded += (s, e) => dataGrid.CommitEdit();
        }
    }
}