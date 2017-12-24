<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="Report">
    <html>
      <head>
        <title>{Report Title}</title>
        <style type="text/css">
          table {
          padding-bottom: 30mm
          }
          .table-title {
          font-weight: 100
          }
          @page {
          size: A4;
          margin: 10mm 10mm 30mm 30mm;
          }
          @media print {
          thead {
          display: table-header-group;
          }
          tr {
          page-break-inside: avoid;
          }
          }
          tr:nth-child(odd) {
          background: White;
          }
          tr:nth-child(even) {
          background: #EFEFEF;
          }
          td {
          list-style: none;
          padding: 10px;
          }
        </style>
      </head>
      <body>
        <table>
          <thead>
            <tr>
              <th colspan="5" class="table-title">Title</th>
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

  <xsl:template match="Encs/Enc">
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
  </xsl:template>
</xsl:stylesheet>
