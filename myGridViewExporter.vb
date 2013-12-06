Imports System
Imports System.Data
Imports System.Configuration
Imports System.IO
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls


Public Class myGridViewExporter

    Public Shared Sub Export(ByVal gv As GridView, ByVal fileName As String)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", fileName))
        HttpContext.Current.Response.ContentType = "application/ms-excel"
        Dim myStringWriter As StringWriter = New StringWriter
        Dim myHTMLTextWriter As HtmlTextWriter = New HtmlTextWriter(myStringWriter)
        '  Create a form to contain the grid
        gv.AllowSorting = False
        gv.AllowPaging = False
        gv.DataBind()
        Dim myTable As Table = New Table
        myTable.GridLines = gv.GridLines
        '  add the header row to the table
        If (Not (gv.HeaderRow) Is Nothing) Then
            myGridViewExporter.PrepareControlForExport(gv.HeaderRow)
            myTable.Rows.Add(gv.HeaderRow)
        End If
        '  add each of the data rows to the table
        For Each row As GridViewRow In gv.Rows
            myGridViewExporter.PrepareControlForExport(row)
            myTable.Rows.Add(row)
        Next
        '  add the footer row to the table
        If (Not (gv.FooterRow) Is Nothing) Then
            myGridViewExporter.PrepareControlForExport(gv.FooterRow)
            myTable.Rows.Add(gv.FooterRow)
        End If
        '  render the table into the htmlwriter
        myTable.RenderControl(myHTMLTextWriter)
        '  render the htmlwriter into the response
        HttpContext.Current.Response.Write(myStringWriter.ToString)
        HttpContext.Current.Response.End()
    End Sub

    ' Replace any of the contained controls with literals
    Private Shared Sub PrepareControlForExport(ByVal control As Control)
        Dim i As Integer = 0
        Do While (i < control.Controls.Count)
            Dim current As Control = control.Controls(i)
            If (TypeOf current Is LinkButton) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, LinkButton).Text))
            ElseIf (TypeOf current Is ImageButton) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, ImageButton).AlternateText))
            ElseIf (TypeOf current Is HyperLink) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, HyperLink).Text))
            ElseIf (TypeOf current Is DropDownList) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, DropDownList).SelectedItem.Text))
            ElseIf (TypeOf current Is CheckBox) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, CheckBox).Checked))
                'TODO: Warning!!!, inline IF is not supported ?
            End If
            If current.HasControls Then
                myGridViewExporter.PrepareControlForExport(current)
            End If
            i = (i + 1)
        Loop
    End Sub


End Class
