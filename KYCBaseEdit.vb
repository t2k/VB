Imports System.Data
Imports System.Data.Entity

Public MustInherit Class KYCBaseEdit
    Inherits System.Web.UI.Page

    Private _formabbr As String

    Public Property KYC_Model As VB.KYCEntities

    ''' <summary>
    ''' KYC CustUniveral entity model object
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KYC_cust As VB.CUSTUniversal



    ''' <summary>
    ''' find KYCID from http ROUTE
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KYCID As Integer

    Public Property KYCUserDept As String

    ''' <summary>
    ''' kyc account owner (Dept)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KYCAcctOwner As String

    ''' <summary>
    ''' user profile abbrieviated name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KYCUserAbbrName As String

    ''' <summary>
    ''' 3 letter abbreviation, will set form title (tabs) like kyc####xyx
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KYCPageAbbr() As String
        Get
            Return _formabbr
        End Get
        Set(value As String)
            _formabbr = value
            Me.Title = String.Format("kyc{0} {1}", KYCID, _formabbr)
        End Set
    End Property


    ''' <summary>
    ''' display edit mode indicator: EDIT/ReadOnly in pull-right header region
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub showEditStatus(ByVal msg As String)
    ''' <summary>
    ''' Show Edit / error messages in header
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub showEditMsg(ByVal msg As String)

    
    Public Function canCreate() As Boolean
        If My.User.IsInRole("KYC-ACCTMGR") Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' validate the authenticated users update rights for this account. Compare user profile role vs 'KYC-{{owner}}'
    ''' </summary>
    ''' <returns>true/false</returns>
    ''' <remarks></remarks>
    Public Function canUpdate() As Boolean
        If My.User.IsInRole(String.Format("KYC-{0}", KYCAcctOwner)) Then
            Return True
        Else
            Return False
        End If

    End Function

    ''' <summary>
    ''' validate the authenticated users CREATE rights for profile role:KYC-ACCTMGR
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLoad(e As System.EventArgs)
        If Not Me.IsPostBack Then
            KYCID = Page.RouteData.Values("id")

            KYC_Model = New VB.KYCEntities

            KYC_cust = (From cu In KYC_Model.CUSTUniversals Where cu.ID = KYCID
                       Select cu).FirstOrDefault

            If KYC_cust Is Nothing Then
                Server.Transfer("~/logout.aspx")
            End If


            KYCAcctOwner = KYC_cust.Owner ' base property

            'Me.Title = String.Format("kyc{0}-AOD", hfID.Value) ' page title tabs so user can see at a glance
            Dim dzUser As DZUserProfile = DZUserProfile.GetUserProfile(My.User.Name)
            KYCUserDept = dzUser.Department
            KYCUserAbbrName = dzUser.AbbrName
            MyBase.OnLoad(e)
        End If

    End Sub


    ''' <summary>
    ''' traverse the server/side DOM for input controls and enable/disable
    ''' </summary>
    ''' <param name="ctrlParent">a control/container like Page</param>
    ''' <param name="state">enabled state True/False</param>
    ''' <remarks></remarks>
    Sub SetControls(ByVal ctrlParent As Control, Optional ByVal state As Boolean = False)
        Dim ctrl As Control
        For Each ctrl In ctrlParent.Controls
            If TypeOf (ctrl) Is CheckBox Then
                CType(ctrl, CheckBox).Enabled = state
            ElseIf TypeOf (ctrl) Is RadioButtonList Then
                CType(ctrl, RadioButtonList).Enabled = state
            ElseIf TypeOf (ctrl) Is TextBox Then
                CType(ctrl, TextBox).Enabled = state
            ElseIf TypeOf (ctrl) Is DropDownList Then
                CType(ctrl, DropDownList).Enabled = state
            End If
            If ctrl.HasControls Then
                SetControls(ctrl, state)
            End If
        Next
    End Sub

End Class
