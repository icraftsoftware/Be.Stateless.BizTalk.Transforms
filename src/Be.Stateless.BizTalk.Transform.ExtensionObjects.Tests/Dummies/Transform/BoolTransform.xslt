<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'
                xmlns:s0="urn:resources:message:bool"
                xmlns:xc='urn:extensions.stateless.be:biztalk:xml:convert:2021:10'>
  <xsl:output omit-xml-declaration='yes' method='xml' version='1.0' />

  <xsl:template match='/s0:Message'>
    <xsl:copy>
      <xsl:apply-templates select='*'/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match='s0:*'>
    <xsl:copy>
      <xsl:apply-templates select='@*' />
      <xsl:value-of select='xc:ToBoolean(text())' />
    </xsl:copy>
  </xsl:template>

  <xsl:template match='@*'>
    <xsl:attribute name='{name(.)}'>
      <xsl:value-of select='xc:ToBoolean(.)' />
    </xsl:attribute>
  </xsl:template>

</xsl:stylesheet>