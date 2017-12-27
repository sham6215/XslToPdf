<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="Report">
    <html>
      <head>
        <title>{Report Title}</title>
        <style type="text/css">
          <style type="text/css">

            table {
            font-size: 14pt;
            }

            .table-title {
            font-weight: 100
            }

            tr:nth-child(odd) {
            background: White;
            }

            tr:nth-child(even) {
            background: #EFEFEF;
            }

            tr {
            page-break-inside: avoid;
            /*display: inline-block;*/
            -webkit-column-break-inside: avoid;
            page-break-inside: avoid;
            break-inside: avoid;

            }

            tr td:not(:nth-child(3)),
            tr th:not(:nth-child(3))  {
            width:1%;
            white-space:nowrap;

            }
          </style>
        </style>
      </head>
      <body>
        <table style="width:100%">
          <thead>
            <tr>
              <th colspan="5" class="table-title">
                <xsl:value-of select="Title" />
              </th>
            </tr>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Description</th>
              <th>Published</th>
              <th>Expired</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="Encs/Enc" >
              <tr>
                <td>
                  <xsl:value-of select="Id" />
                </td>
                <td>
                  <xsl:value-of select="Name" />
                </td>
                <td>
                  <xsl:value-of select="Description" />
                </td>
                <td>
                  <xsl:value-of select="PublicationDate" />
                </td>
                <td>
                  <xsl:value-of select="ExpiryDate" />
                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
