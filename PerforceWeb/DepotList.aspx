<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepotList.aspx.cs" Inherits="PerforceWeb.DepotList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Perforce Web Depot Interface</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="container">
        <div class="col-md-4">
                <h4>Select File(s) from P4:</h4>
                <asp:TreeView ID="p4Tree" runat="server" OnTreeNodeExpanded="p4Tree_TreeNodeExpanded" Height="500px" style="overflow: auto; font-size:small" ExpandDepth="0" BorderColor="#642891" BorderStyle="Solid" BorderWidth="1px"></asp:TreeView>
            </div> 
    
    </div>
    </form>
</body>
</html>
