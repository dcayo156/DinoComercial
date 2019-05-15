Imports Logica.AccesoLogica
Imports DevComponents.DotNetBar
Public Class Pr_StockUtilidad

    'gb_FacturaIncluirICE

    Public _nameButton As String
    Public _tab As SuperTabItem
    Dim bandera As Boolean = False

    Public Sub _prIniciarTodo()
        L_prAbrirConexion(gs_Ip, gs_UsuarioSql, gs_ClaveSql, gs_NombreBD)
        _prCargarComboLibreriaSucursal(tbAlmacen)
        _prCargarComboLibreriaPrecioVenta(tbcatprecio)
        _PMIniciarTodo()
        Me.Text = "UTILIDAD DE PRODUCTOS"
        MReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        _IniciarComponentes()
        bandera = True
    End Sub
    Public Sub _IniciarComponentes()

    End Sub
    Public Sub _prInterpretarDatos(ByRef _dt As DataTable)
        If (tbAlmacen.SelectedIndex >= 0 And tbcatprecio.SelectedIndex >= 0 And Checktodos.Checked) Then
            _dt = L_prReporteUtilidad(tbAlmacen.Value, tbcatprecio.Value)
        End If

        If (tbAlmacen.SelectedIndex >= 0 And tbcatprecio.SelectedIndex >= 0 And checkMayorCero.Checked) Then
            _dt = L_prReporteUtilidadStockMayorCero(tbAlmacen.Value, tbcatprecio.Value)

        End If
    End Sub
    Private Sub _prCargarReporte()
        Dim _dt As New DataTable
        _prInterpretarDatos(_dt)
        If (_dt.Rows.Count > 0) Then

            Dim objrep As New R_StockActualUtilidad
            objrep.SetDataSource(_dt)

            objrep.SetParameterValue("usuario", L_Usuario)
            objrep.SetParameterValue("categoria", tbAlmacen.Text + " - " + tbcatprecio.Text)
            MReportViewer.ReportSource = objrep
            MReportViewer.Show()
            MReportViewer.BringToFront()



        Else
            ToastNotification.Show(Me, "NO HAY DATOS PARA LOS PARAMETROS SELECCIONADOS..!!!",
                                       My.Resources.INFORMATION, 2000,
                                       eToastGlowColor.Blue,
                                       eToastPosition.BottomLeft)
            MReportViewer.ReportSource = Nothing
        End If





    End Sub
    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        _prCargarReporte()

    End Sub

    Private Sub Pr_VentasAtendidas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _prIniciarTodo()

    End Sub






    Private Sub _prCargarComboLibreriaSucursal(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo)
        Dim dt As New DataTable
        dt = L_fnListarSucursales()
        With mCombo
            .DropDownList.Columns.Clear()
            .DropDownList.Columns.Add("aanumi").Width = 60
            .DropDownList.Columns("aanumi").Caption = "COD"
            .DropDownList.Columns.Add("aabdes").Width = 500
            .DropDownList.Columns("aabdes").Caption = "SUCURSAL"
            .ValueMember = "aanumi"
            .DisplayMember = "aabdes"
            .DataSource = dt
            .Refresh()
        End With
        If (dt.Rows.Count > 0) Then
            tbAlmacen.SelectedIndex = 0
        End If
    End Sub

    Private Sub _prCargarComboLibreriaPrecioVenta(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo)
        Dim dt As New DataTable
        dt = L_prListarPrecioVenta()
        With mCombo
            .DropDownList.Columns.Clear()
            .DropDownList.Columns.Add("ygnumi").Width = 60
            .DropDownList.Columns("ygnumi").Caption = "COD"
            .DropDownList.Columns.Add("ygdesc").Width = 500
            .DropDownList.Columns("ygdesc").Caption = "SUCURSAL"
            .ValueMember = "ygnumi"
            .DisplayMember = "ygdesc"
            .DataSource = dt
            .Refresh()
        End With
        If (dt.Rows.Count > 0) Then
            tbcatprecio.SelectedIndex = 0
        End If
    End Sub
    Private Sub _prCargarComboLibreriaPrecioCosto(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo)
        Dim dt As New DataTable
        dt = L_prListarPrecioCosto()
        With mCombo
            .DropDownList.Columns.Clear()
            .DropDownList.Columns.Add("ygnumi").Width = 60
            .DropDownList.Columns("ygnumi").Caption = "COD"
            .DropDownList.Columns.Add("ygdesc").Width = 500
            .DropDownList.Columns("ygdesc").Caption = "SUCURSAL"
            .ValueMember = "ygnumi"
            .DisplayMember = "ygdesc"
            .DataSource = dt
            .Refresh()
        End With
        If (dt.Rows.Count > 0) Then
            tbcatprecio.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click

        _tab.Close()

    End Sub

    Private Sub swTipoVenta_ValueChanged(sender As Object, e As EventArgs) Handles swTipoVenta.ValueChanged
        If (bandera = False) Then
            Return
        End If
        If (swTipoVenta.Value = True) Then
            _prCargarComboLibreriaPrecioVenta(tbcatprecio)
        Else
            _prCargarComboLibreriaPrecioCosto(tbcatprecio)

        End If

    End Sub

End Class