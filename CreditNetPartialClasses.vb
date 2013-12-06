
Namespace DataSetInterfaceTableAdapters
    ' these are two large interface tables that need to get updated daily and the update querytime can exceed the 30sec. default timeout
    Partial Public Class mpCDSCurveTableAdapter
        Sub SetCommandTimeout(ByVal _timeoutSeconds As Integer)
            For Each Command As System.Data.SqlClient.SqlCommand In Me.CommandCollection
                Command.CommandTimeout = _timeoutSeconds
            Next
        End Sub
    End Class

    Partial Public Class SPRatingsIFaceTableAdapter
        Sub SetCommandTimeout(ByVal _timeoutSeconds As Integer)
            For Each Command As System.Data.SqlClient.SqlCommand In Me.CommandCollection
                Command.CommandTimeout = _timeoutSeconds
            Next
        End Sub
    End Class

End Namespace

Namespace DataSetBespokeTableAdapters
    Partial Public Class PortfolioSPIndustryTableAdapter
        Sub SetCommandTimeout(ByVal _timeoutSeconds As Integer)
            For Each Command As System.Data.SqlClient.SqlCommand In Me.CommandCollection
                Command.CommandTimeout = _timeoutSeconds
            Next
        End Sub
    End Class

    Partial Public Class PortfolioSPTableAdapter
        Sub SetCommandTimeout(ByVal _timeoutSeconds As Integer)
            For Each Command As System.Data.SqlClient.SqlCommand In Me.CommandCollection
                Command.CommandTimeout = _timeoutSeconds
            Next
        End Sub
    End Class

    Partial Public Class PortfolioZSCORETop10Pct
        Sub SetCommandTimeout(ByVal _timeoutSeconds As Integer)
            For Each Command As System.Data.SqlClient.SqlCommand In Me.CommandCollection
                Command.CommandTimeout = _timeoutSeconds
            Next
        End Sub
    End Class


End Namespace
