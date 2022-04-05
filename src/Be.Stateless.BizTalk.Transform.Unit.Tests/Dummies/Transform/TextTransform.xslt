<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" />

  <xsl:template match="*[position()=last()]">
    <xsl:value-of select="local-name()" />
    <xsl:text>&#13;</xsl:text>
    <xsl:apply-templates select="*"/>
  </xsl:template>

  <xsl:template match="*[position() mod 2 = 0]">
    <xsl:value-of select="local-name()" />
    <xsl:text>&#13;</xsl:text>
    <xsl:apply-templates select="*"/>
  </xsl:template>

  <xsl:template match="*">
    <xsl:value-of select="local-name()" />
    <xsl:text>, </xsl:text>
    <xsl:apply-templates select="*"/>
  </xsl:template>

</xsl:stylesheet>