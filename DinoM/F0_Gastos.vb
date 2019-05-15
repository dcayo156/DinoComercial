Imports Logica.AccesoLogica
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls
Imports Janus.Windows.GridEX
Imports System.IO
Imports DevComponents.DotNetBar.SuperGrid

Public Class F0_Gastos
#Region "Variables locales"
    Public _nameButton As String
    Public _modulo As SideNavItem
    Dim Modificado As Boolean = False
    Public _tab As SuperTabItem

#End Region

#Region "Eventos"
    Public Sub Method()

    End Sub

    Private Sub F0_Gastos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _prIniciarTodo()
    End Sub
    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        If btnGrabar.Enabled = True Then
            _PMInhabilitar()
            _PMPrimerRegistro()

        Else
            '  Public _modulo As SideNavItem
            _modulo.Select()
            _tab.Close()
        End If
    End Sub
#End Region

#Region "Metodos privados"
    Private Sub _prIniciarTodo()
        L_prAbrirConexion(gs_Ip, gs_UsuarioSql, gs_ClaveSql, gs_NombreBD)
        Me.Text = "GASTOS"
        _prMaxLength()
        _prAsignarPermisos()
        _PMIniciarTodo()

        Dim blah As New Bitmap(New Bitmap(My.Resources.producto), 20, 20)
        Dim ico As Icon = Icon.FromHandle(blah.GetHicon())
        Me.Icon = ico
    End Sub
    Public Sub _prStyleJanus()
        GroupPanelBuscador.Style.BackColor = Color.FromArgb(13, 71, 161)
        GroupPanelBuscador.Style.BackColor2 = Color.FromArgb(13, 71, 161)
        GroupPanelBuscador.Style.TextColor = Color.White
        JGrM_Buscador.RootTable.HeaderFormatStyle.FontBold = TriState.True
    End Sub
    Public Sub _prMaxLength()
        tbConcepto.MaxLength = 20
        tbDescripcion.MaxLength = 50
        TbMonto.MaxValue = 10000000
    End Sub
    Private Sub _prAsignarPermisos()

        Dim dtRolUsu As DataTable = L_prRolDetalleGeneral(gi_userRol, _nameButton)

        Dim show As Boolean = dtRolUsu.Rows(0).Item("ycshow")
        Dim add As Boolean = dtRolUsu.Rows(0).Item("ycadd")
        Dim modif As Boolean = dtRolUsu.Rows(0).Item("ycmod")
        Dim del As Boolean = dtRolUsu.Rows(0).Item("ycdel")

        If add = False Then
            btnNuevo.Visible = False
        End If
        If modif = False Then
            btnModificar.Visible = False
        End If
        If del = False Then
            btnEliminar.Visible = False
        End If
    End Sub
    'Private Sub _prSalir()
    '    If btnGrabar.Enabled = True Then
    '        _PMOInhabilitar()
    '        _PMPrimerRegistro()
    '    Else
    '        _modulo.Select()
    '        _tab.Close()
    '    End If
    'End Sub

#End Region
#Region "Metodos sobrecargados"

    Public Overrides Sub _PMOHabilitar()
        tbConcepto.ReadOnly = False
        tbDescripcion.ReadOnly = False
        TbMonto.IsInputReadOnly = False
        tbFechaVenta.IsInputReadOnly = False
        tbConcepto.Focus()
    End Sub
    Public Overrides Sub _PMOInhabilitar()
        tbCodigoOriginal.ReadOnly = True
        tbConcepto.ReadOnly = True
        tbDescripcion.ReadOnly = True
        TbMonto.IsInputReadOnly = True
        tbFechaVenta.IsInputReadOnly = True
        _prStyleJanus()
        JGrM_Buscador.Focus()
    End Sub
    Public Overrides Sub _PMOLimpiar()
        tbCodigoOriginal.Clear()
        tbConcepto.Clear()
        tbDescripcion.Clear()
        TbMonto.Value = 0
        tbFechaVenta.Value = DateTime.Today
    End Sub
    Public Overrides Sub _PMOLimpiarErrores()
        MEP.Clear()
        tbConcepto.BackColor = Color.White
    End Sub
    Public Overrides Function _PMOGrabarRegistro() As Boolean


        'ByRef ganumi As String, gacon As String, gadesc As String, gamont As Double, gafact As String, gahact As String, gafdoc As Date) As Boolean
        Dim res As Boolean = L_FnGrabarGastos(tbCodigoOriginal.Text, tbConcepto.Text, tbDescripcion.Text, TbMonto.Value, Now.Date.ToString("yyyy/MM/dd"), TimeString.ToString(), tbFechaVenta.Value.ToString("yyyy/MM/dd"))

        If res Then
            Modificado = False
            Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
            ToastNotification.Show(Me, "Código de Gasto ".ToUpper + tbCodigoOriginal.Text + " Grabado con Exito.".ToUpper,
                                      img, 2000,
                                      eToastGlowColor.Green,
                                      eToastPosition.TopCenter
                                      )
            tbConcepto.Focus()

        Else
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me, "El Gasto no pudo ser insertado".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)

        End If
        Return res
    End Function
    'Modificar
    Public Overrides Function _PMOModificarRegistro() As Boolean
        Dim res As Boolean

        If (Modificado = False) Then
            res = L_FnModificarGastos(tbCodigoOriginal.Text, tbConcepto.Text, tbDescripcion.Text, TbMonto.Value, tbFechaVenta.Value.ToString("yyyy/MM/dd"), Now.Date.ToString("yyyy/MM/dd"), lbHora.Text)
        Else
            res = L_FnModificarGastos(tbCodigoOriginal.Text, tbConcepto.Text, tbDescripcion.Text, TbMonto.Value, tbFechaVenta.Value.ToString("yyyy/MM/dd"), Now.Date.ToString("yyyy/MM/dd"), lbHora.Text)
        End If
        If res Then

            If (Modificado = True) Then
                Modificado = False
            End If
            Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
            ToastNotification.Show(Me, "Código de Gastos ".ToUpper + tbCodigoOriginal.Text + " modificado con Exito.".ToUpper,
                                      img, 2000,
                                      eToastGlowColor.Green,
                                      eToastPosition.TopCenter)

        Else
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me, "EL Gasto no pudo ser modificado".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)

        End If
        _PMInhabilitar()
        _PMPrimerRegistro()
        Return res
    End Function
    Public Function _fnActionNuevo() As Boolean
        Return tbCodigoOriginal.Text = String.Empty And tbConcepto.ReadOnly = False
    End Function
    'Eliminar
    Public Overrides Sub _PMOEliminarRegistro()

        Dim ef = New Efecto
        ef.tipo = 2
        ef.Context = "¿esta seguro de eliminar el registro?".ToUpper
        ef.Header = "mensaje principal".ToUpper
        ef.ShowDialog()
        Dim bandera As Boolean = False
        bandera = ef.band
        If (bandera = True) Then
            Dim mensajeError As String = ""
            Dim res As Boolean = L_FnEliminarGastos(tbCodigoOriginal.Text, mensajeError)
            If res Then

                Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
                ToastNotification.Show(Me, "Código de Gastos ".ToUpper + tbCodigoOriginal.Text + " eliminado con Exito.".ToUpper,
                                          img, 2000,
                                          eToastGlowColor.Green,
                                          eToastPosition.TopCenter)

                _PMFiltrar()
            Else
                Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
                ToastNotification.Show(Me, mensajeError, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            End If
        End If
    End Sub
    'Validar campos
    Public Overrides Function _PMOValidarCampos() As Boolean
        Dim _ok As Boolean = True
        MEP.Clear()

        If tbConcepto.Text = String.Empty Then
            tbConcepto.BackColor = Color.Red
            MEP.SetError(tbConcepto, "ingrese el concepto del Gasto!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
            ToastNotification.Show(Me, "Ingrese el concepto del gasto para efectuar la grabacion".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            tbConcepto.BackColor = Color.White
            MEP.SetError(tbConcepto, "")
        End If

        If TbMonto.Text = "0,00" Then
            TbMonto.BackColor = Color.Red
            MEP.SetError(TbMonto, "Ingrese un monto!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
            ToastNotification.Show(Me, "Ingrese un monto mayor a 0".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            TbMonto.BackColor = Color.White
            MEP.SetError(TbMonto, "")
        End If
        MHighlighterFocus.UpdateHighlights()
        Return _ok
    End Function
    Public Overrides Function _PMOGetTablaBuscador() As DataTable
        Dim dtBuscador As DataTable = L_FnGeneralGastos()
        Return dtBuscador
    End Function

    Public Overrides Function _PMOGetListEstructuraBuscador() As List(Of Modelo.Celda)
        Dim listEstCeldas As New List(Of Modelo.Celda)
        'a.abnumi ,abdesc ,a.abdir ,a.abtelf ,a.ablat ,a.ablong ,a.abimg,abest ,Cast(abest as bit) as estado,a.abfact ,a.abhact ,a.abuact

        listEstCeldas.Add(New Modelo.Celda("ganumi", True, "Código".ToUpper, 80))
        listEstCeldas.Add(New Modelo.Celda("gacon", True, "Concepto".ToUpper, 150))
        listEstCeldas.Add(New Modelo.Celda("gadesc", True, "Descripcion".ToUpper, 250))
        listEstCeldas.Add(New Modelo.Celda("gamont", True, "Monto".ToUpper, 130))
        listEstCeldas.Add(New Modelo.Celda("gafdoc", True, "Fecha".ToUpper, 120))
        listEstCeldas.Add(New Modelo.Celda("gafact", False, "FechaActual".ToUpper, 40))
        listEstCeldas.Add(New Modelo.Celda("gahact", False, "Hora".ToUpper, 40))
        listEstCeldas.Add(New Modelo.Celda("gauact", False, "Usuario".ToUpper, 40))

        Return listEstCeldas
    End Function

    'Mostrar registro
    Public Overrides Sub _PMOMostrarRegistro(_N As Integer)
        JGrM_Buscador.Row = _MPos
        'a.aanumi ,a.aabdes ,a.aadir ,a.aatel ,a.aalat ,a.aalong ,a.aaimg,aata2dep ,a.aafact ,a.aahact ,a.aauact
        Dim dt As DataTable = CType(JGrM_Buscador.DataSource, DataTable)
        Try
            tbCodigoOriginal.Text = JGrM_Buscador.GetValue("ganumi").ToString
        Catch ex As Exception
            Exit Sub
        End Try
        With JGrM_Buscador
            tbCodigoOriginal.Text = .GetValue("ganumi").ToString
            tbConcepto.Text = .GetValue("gacon")
            tbDescripcion.Text = .GetValue("gadesc")
            TbMonto.Value = .GetValue("gamont")
            tbFechaVenta.Text = CType(.GetValue("gafdoc"), Date).ToString("dd/MM/yyyy")
            lbFecha.Text = CType(.GetValue("gafact"), Date).ToString("dd/MM/yyyy")
            lbHora.Text = .GetValue("gahact").ToString
            lbUsuario.Text = .GetValue("gauact").ToString

        End With
        LblPaginacion.Text = Str(_MPos + 1) + "/" + JGrM_Buscador.RowCount.ToString

    End Sub






    'Modificar






#End Region

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

    End Sub

End Class