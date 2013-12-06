Imports System.Data
Imports System.Linq
Imports System.Web.Security
Imports System.Web.Profile
Imports System.Collections





' create a standard User profile class from the Profile Base
' NEEDED: FirstName, LastName, AbbrName, Department 
' for starters
' TO DO: add: Country, Center  ie Germany, Frankfurt, US, New York
Public Class DZKYCUserReport
    Public Property UserName As String
    Public Property FirstName As String
    Public Property LastName As String
    Public Property AbbrName As String
    Public Property Department As String
    Public Property BankingCenter As String
    Public Property Email As String
    Public Property PasswordChange As Date
    Public Property LastLogin As Date
    Public Property IsLockedOut As Boolean
    Public Property IsApproved As Boolean
End Class

Public Class DZUserProfileList
    Public Property UserName As String
    Public Property Email As String
    Public Property FirstName As String
    Public Property Last As String
    Public Property AbbrName As String
    Public Property Department As String
    Public Property BaningCenter As String
    Public Property Phone As String
    Public Property CreationDate As Date
    Public Property LastLoginDate As Date
    Public Property IsOnline As Boolean
    Public Property IsLockedOut As Boolean
    Public Property IsApproved As Boolean
End Class

Public Class zirpenProfileQik
    Public Property userId As String
    Public Property fullName As String
    Public Property profileImg As String ' string to profile image 
End Class



Public Enum Gender
    Male
    Female
End Enum

Public Class DZUserProfile
    Inherits ProfileBase

    'Public Sub New()
    '    MyBase.New()
    'End Sub



    <SettingsAllowAnonymous(False)>
    Public Property FirstName() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("FirstName"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("FirstName", value)
        End Set
    End Property

    <SettingsAllowAnonymous(False)>
    Public Property LastName() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("LastName"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("LastName", value)
        End Set
    End Property

    <SettingsAllowAnonymous(False)>
    Public ReadOnly Property FullName() As String
        Get
            Return String.Format("{0} {1}", TryCast(MyBase.GetPropertyValue("FirstName"), String), TryCast(MyBase.GetPropertyValue("LastName"), String))
        End Get
    End Property


    <SettingsAllowAnonymous(False)>
    Public Property Gender() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("Gender"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("Gender", value)
        End Set
    End Property


    <SettingsAllowAnonymous(False)>
    Public Property AbbrName() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("AbbrName"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("AbbrName", value)
        End Set
    End Property

    <SettingsAllowAnonymous(False)>
    Public Property Department() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("Department"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("Department", value)
        End Set
    End Property

    <SettingsAllowAnonymous(False)>
    Public Property TimeZone() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("TimeZone"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("TimeZone", value)
        End Set
    End Property


    <SettingsAllowAnonymous(False)>
    Public Property Picture() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("Picture"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("Picture", value)
        End Set
    End Property

    <SettingsAllowAnonymous(False)>
    Public Property Culture() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("Culture"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("Culture", value)
        End Set
    End Property

    <SettingsAllowAnonymous(False)>
    Public Property BankingCenter() As String
        Get
            Return TryCast(MyBase.GetPropertyValue("BankingCenter"), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue("BankingCenter", value)
        End Set
    End Property

    ''' <summary>
    ''' get the profile of a specific user (admin usage)
    ''' </summary>
    ''' <param name="_UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserProfile(ByVal _UserName As String) As DZUserProfile
        Return TryCast(Create(_UserName), DZUserProfile)
    End Function


    ''' <summary>
    ''' Get profile of current logged on user
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserProfile() As DZUserProfile
        Return TryCast(HttpContext.Current.Profile, DZUserProfile)
        ' Return TryCast(Create(Membership.GetUser().UserName), DZUserProfile)
    End Function


    Function FindUsersByEMail(ByVal _strFilter As String) As IEnumerable
        Return From u In Membership.FindUsersByEmail(_strFilter)
        Select u.UserName, u.Email, GetUserProfile(u.UserName).FirstName,
        GetUserProfile(u.UserName).LastName, GetUserProfile(u.UserName).AbbrName,
        GetUserProfile(u.UserName).Department, u.Comment, u.CreationDate,
        u.LastLoginDate, u.IsOnline, u.IsLockedOut Order By Department, LastName
    End Function


    ' return comma seperate list of email addresses for all users within a given _role
    Function GetEMailInRole(ByVal _role As String) As String
        Dim strRecipients As String = ""

        Try
            For Each strUser As String In Roles.GetUsersInRole(_role)
                strRecipients += Membership.GetUser(strUser).Email + ","
            Next
            Return strRecipients.TrimEnd(",")
        Catch ex As Exception
            Return ""
        End Try
    End Function


    ' return a datatable for better sorting...
    ' probably not the most efficient method, but it'll work for smaller lists
    Function GetDZProfileByLetter(ByVal _letter As String) As IEnumerable
        If _letter = "%" Then
            Return From u In Membership.GetAllUsers
            Select u.UserName,
            u.Email,
            GetUserProfile(u.UserName).FirstName,
            GetUserProfile(u.UserName).LastName,
            GetUserProfile(u.UserName).AbbrName,
            GetUserProfile(u.UserName).Department,
            GetUserProfile(u.UserName).BankingCenter,
            Phone = u.Comment,
            u.CreationDate,
            u.LastLoginDate,
            u.LastPasswordChangedDate,
            u.IsOnline,
            u.IsLockedOut,
            u.IsApproved
            Order By LastName, BankingCenter
        Else
            Return From u In Membership.GetAllUsers
            Select u.UserName,
            u.Email,
            GetUserProfile(u.UserName).FirstName,
            GetUserProfile(u.UserName).LastName,
            GetUserProfile(u.UserName).AbbrName,
            GetUserProfile(u.UserName).Department,
            GetUserProfile(u.UserName).BankingCenter,
            Phone = u.Comment,
            u.CreationDate,
            u.LastLoginDate,
            u.LastPasswordChangedDate,
            u.IsOnline,
            u.IsLockedOut,
            u.IsApproved
            Order By BankingCenter, LastName
            Where LastName.StartsWith(_letter.Substring(0, 1))
        End If
    End Function



    ''' <summary>
    ''' Report of 'Approved' users in a role
    ''' </summary>
    ''' <param name="_role"></param>
    ''' <returns>ienumerable list</returns>
    ''' <remarks></remarks>
    Function rptUsersInRole(_role As String) As List(Of DZKYCUserReport) ' IEnumerable
        ' filter membership collection by role
        Dim qry = (From u As MembershipUser In Membership.GetAllUsers
        Where Roles.GetUsersInRole(_role).Contains(u.UserName)
        Select u).ToList()

        ' get typed object and load fields from membership and UserProfile (two different things) 
        ' note: DZKYCUserReport is a class in this module/file...
        Dim qry2 = (From r In qry Select New DZKYCUserReport() With {
               .UserName = r.UserName,
               .FirstName = GetUserProfile(r.UserName).FirstName,
               .LastName = GetUserProfile(r.UserName).LastName,
               .AbbrName = GetUserProfile(r.UserName).AbbrName,
               .Department = GetUserProfile(r.UserName).Department,
               .BankingCenter = GetUserProfile(r.UserName).BankingCenter,
               .Email = r.Email,
               .LastLogin = r.LastLoginDate,
               .PasswordChange = r.LastPasswordChangedDate,
               .IsApproved = r.IsApproved,
               .IsLockedOut = r.IsLockedOut
           }).ToList()

        ' sort and return
        Return (From z In qry2
               Order By z.BankingCenter, z.Department, z.LastName
               Select z).ToList()

    End Function

    ''' <summary>
    '''  return a list of DZKYCUserReport objects
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function rptActiveUsers() As List(Of DZKYCUserReport)
        Dim qry = (From u As MembershipUser In Membership.GetAllUsers
                   Select u).ToList()

        Dim qry2 = (From r In qry Select New DZKYCUserReport() With {
               .UserName = r.UserName,
               .FirstName = GetUserProfile(r.UserName).FirstName,
               .LastName = GetUserProfile(r.UserName).LastName,
               .AbbrName = GetUserProfile(r.UserName).AbbrName,
               .Department = GetUserProfile(r.UserName).Department,
               .BankingCenter = GetUserProfile(r.UserName).BankingCenter,
               .Email = r.Email,
               .LastLogin = r.LastLoginDate,
               .PasswordChange = r.LastPasswordChangedDate,
               .IsApproved = r.IsApproved,
               .IsLockedOut = r.IsLockedOut
           }).ToList()

        Return (From z In qry2
                Order By z.LastName
                Select z).ToList()
    End Function

    Function rptActiveUsersByLoginDate() As List(Of DZKYCUserReport)
        Dim qry = (From u As MembershipUser In Membership.GetAllUsers
                   Select u).ToList()

        Dim qry2 = (From r In qry Select New DZKYCUserReport() With {
               .UserName = r.UserName,
               .FirstName = GetUserProfile(r.UserName).FirstName,
               .LastName = GetUserProfile(r.UserName).LastName,
               .AbbrName = GetUserProfile(r.UserName).AbbrName,
               .Department = GetUserProfile(r.UserName).Department,
               .BankingCenter = GetUserProfile(r.UserName).BankingCenter,
               .Email = r.Email,
               .LastLogin = r.LastLoginDate,
               .PasswordChange = r.LastPasswordChangedDate,
               .IsApproved = r.IsApproved,
               .IsLockedOut = r.IsLockedOut
           }).ToList()

        Return (From z In qry2
                Order By z.LastLogin Descending, z.LastName
                Select z).ToList()
    End Function


    Function GetAllDZUsers() As IEnumerable
        'LINQ 

        Return From u In Membership.GetAllUsers
        Select u.UserName, u.Email, GetUserProfile(u.UserName).FirstName,
        LastName = GetUserProfile(u.UserName).LastName, GetUserProfile(u.UserName).AbbrName,
        GetUserProfile(u.UserName).Department, u.Comment, u.CreationDate,
        u.LastLoginDate, u.IsOnline, u.IsLockedOut Order By LastName

    End Function


    ' T. Killilea Jan 28 08 
    ' return a collection of unique departments from the private profile
    'Function UniqueDepartments() As ICollection
    ' new 7/22/09 added LINQ support...
    ' mark, check it out... one line of code vs the old way
    Function UniqueDepartments() As IEnumerable
        Return From usr In Membership.GetAllUsers Select GetUserProfile(usr.UserName).Department Order By Department Distinct

        'Dim sl As New SortedList
        'Dim mu As MembershipUserCollection = Membership.GetAllUsers

        'For Each u As MembershipUser In mu
        '    Try
        '        Dim dzuser As DZUserProfile = GetUserProfile(u.UserName) ' = GetUserProfile(u.UserName) ' DZUserProfile.Create(u.UserName)
        '        Dim strDept As String = dzuser.Department
        '        If Not sl.Contains(strDept) Then
        '            sl.Add(strDept, strDept)
        '        End If
        '    Catch ex As Exception
        '    End Try
        'Next
        'Return sl.Values
    End Function


    ' NOTE: this is a HACK whereby we using department/Center for all non NY users... 
    ' this will be deprecated by adding new field say "Center" to the profile and store for each ie NY, London, Frankfurt, Singapore etc
    Public Shared Function UniqueNYDepartments() As IEnumerable
        Return From u In Membership.GetAllUsers Where Not GetUserProfile(u.UserName).Department.Contains("/") Select GetUserProfile(u.UserName).Department Order By Department Distinct
    End Function


    ' T. Killilea Jan 28 08 
    ' return the AbbrName in a _departmetn
    ' this is not safe and should be deprecated... should only retieve username
    ' NEW: Sep. 2010, filter by country from updated profile
    Public Shared Function AbbrNameInDeptartment(ByVal _department As String) As UTIL.DZUserProfileDataTable
        Dim retlist As Object

        If _department = "%" Then
            retlist = From u As MembershipUser In Membership.GetAllUsers Where u.IsApproved And GetUserProfile(u.UserName).BankingCenter = "New York" Order By GetUserProfile(u.UserName).LastName Select u.UserName, GetUserProfile(u.UserName).AbbrName, GetUserProfile(u.UserName).Department, u.Email
        Else
            retlist = From u As MembershipUser In Membership.GetAllUsers Where u.IsApproved And _department = GetUserProfile(u.UserName).Department And GetUserProfile(u.UserName).BankingCenter = "New York" Order By GetUserProfile(u.UserName).LastName Select u.UserName, GetUserProfile(u.UserName).AbbrName, GetUserProfile(u.UserName).Department, u.Email
        End If
        Dim dt As New UTIL.DZUserProfileDataTable
        For Each li In retlist
            Dim row As UTIL.DZUserProfileRow = dt.NewDZUserProfileRow
            With row
                .UserName = li.UserName
                .EMail = li.Email
                .AbbrName = li.AbbrName
                .Department = li.Department
            End With
            dt.AddDZUserProfileRow(row)
        Next
        Return dt
    End Function

    Public Shared Function AbbrNames() As UTIL.DZUserProfileDataTable
        Dim retlist = From u As MembershipUser In Membership.GetAllUsers Where u.IsApproved Order By GetUserProfile(u.UserName).LastName Select u.UserName, GetUserProfile(u.UserName).AbbrName, GetUserProfile(u.UserName).Department, u.Email
        Dim dt As New UTIL.DZUserProfileDataTable
        For Each li In retlist
            Dim row As UTIL.DZUserProfileRow = dt.NewDZUserProfileRow
            With row
                .UserName = li.UserName
                .EMail = li.Email
                .AbbrName = li.AbbrName
                .Department = li.Department
            End With
            dt.AddDZUserProfileRow(row)
        Next
        Return dt
    End Function



    ' SHOULD BE DEPRECATED, used 
    Function AbbrNameInDeptartment2(ByVal _department As String) As UTIL.DZUserProfileDataTable
        Dim sl As New SortedList


        Dim mu As MembershipUserCollection = Membership.GetAllUsers
        Dim dt As New UTIL.DZUserProfileDataTable

        For Each u As MembershipUser In mu
            Dim dzUser As DZUserProfile = GetUserProfile(u.UserName)

            If _department = "%" Then
                If u.IsApproved = True Then

                    Try
                        sl.Add(dzUser.LastName, dzUser.UserName)
                    Catch ex As Exception
                        Try
                            sl.Add(dzUser.LastName + " ", dzUser.UserName)
                        Catch
                            sl.Add(dzUser.LastName + "  ", dzUser.UserName)
                        End Try
                    End Try
                End If
            ElseIf _department.ToLower = dzUser.Department.ToLower Then
                If u.IsApproved = True Then
                    Try

                        sl.Add(dzUser.LastName, dzUser.UserName)
                    Catch ex As Exception
                        Try
                            sl.Add(dzUser.LastName + " ", dzUser.UserName)
                        Catch
                            sl.Add(dzUser.LastName + "  ", dzUser.UserName)
                        End Try
                    End Try
                End If
            ElseIf _department = "%" Then
            End If
        Next

        For Each de As DictionaryEntry In sl
            Dim dzUser As DZUserProfile = DZUserProfile.Create(de.Value)
            Dim row As UTIL.DZUserProfileRow = dt.NewDZUserProfileRow
            With row
                .UserName = dzUser.UserName
                .AbbrName = dzUser.AbbrName
                .Department = dzUser.Department
            End With
            dt.AddDZUserProfileRow(row)
        Next

        Return dt
    End Function

    Public Shared Function GetMgrs(ByVal _dept As String) As String
        Dim strRecipients As String = ""
        Try
            For Each strUser As String In Roles.GetUsersInRole("EVENTS-DEPTMGRS")
                'Dim dzUser As DZUserProfile = GetUserProfile(strUser)
                If GetUserProfile(strUser).Department.ToLower = _dept.ToLower Then
                    strRecipients += Membership.GetUser(strUser).Email + ","
                End If
            Next
            Return strRecipients.TrimEnd(",")
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ' NOTE: This should be deprecated, it is not save to use abbr name (can't be sure it's unique)
    ' what would happen if we have 5 persons named: Bob Smith
    Public Shared Function GetEmailbyAbbrName(ByVal _abbrName As String) As String
        Dim strRecipients As String = ""
        Try
            Dim mu As MembershipUserCollection = Membership.GetAllUsers
            For Each u As MembershipUser In mu
                'Dim dzUser As DZUserProfile = GetUserProfile(u.UserName)
                If _abbrName.ToLower.TrimEnd = GetUserProfile(u.UserName).AbbrName.ToLower.TrimEnd Then
                    strRecipients += u.Email + ","
                End If
            Next
            Return strRecipients.TrimEnd(",")
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function GetUserEMail(ByVal _user As String) As String
        Return Membership.GetUser(_user).Email
    End Function

    Public Shared Function GetUserDept(ByVal _userName As String) As String
        Try
            Return GetUserProfile(_userName).Department
        Catch ex As Exception
            Return ""
        End Try
    End Function


    Public Shared Function ContactDDLDepartment(ByVal _UserName As String) As UTIL.DZUserProfileDataTable
        Dim dt As New UTIL.DZUserProfileDataTable

        Dim row1 As UTIL.DZUserProfileRow = dt.NewDZUserProfileRow
        With row1
            .AbbrName = "Branch"
            .UserName = _UserName
            .Department = "Branch"
            .EMail = "xx"
        End With
        dt.AddDZUserProfileRow(row1)

        Dim row2 As UTIL.DZUserProfileRow = dt.NewDZUserProfileRow
        With row2
            .AbbrName = "Branch"
            .UserName = _UserName
            .Department = GetUserDept(_UserName)
            .EMail = "xx"
        End With
        dt.AddDZUserProfileRow(row2)
        Return dt
    End Function

    ''' <summary>
    ''' Pass in a List(of String) of the users followers to obtain a quick profile (id, fullname, picture)
    ''' </summary>
    ''' <param name="users"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getZirpenProfilesQik(ByVal users As List(Of String)) As List(Of zirpenProfileQik)
        Dim qry = From u As MembershipUser In Membership.GetAllUsers
                  Where users.Contains(u.UserName, StringComparer.OrdinalIgnoreCase)
                  Select New zirpenProfileQik() With {
                    .userId = u.UserName,
                    .fullName = String.Format("{0} {1}", GetUserProfile(u.UserName).FirstName, GetUserProfile(u.UserName).LastName),
                    .profileImg = GetUserProfile(u.UserName).Picture
                  }
        Return qry.ToList()
    End Function




End Class

