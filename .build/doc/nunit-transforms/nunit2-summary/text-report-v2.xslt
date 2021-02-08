<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method='text'/>
  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="test-results">
    <xsl:text>***** </xsl:text>
    <xsl:value-of select="@name"/>
    <xsl:text>&#xD;&#xA;&#xD;&#xA;</xsl:text>
    
    <xsl:text>NUnit Version </xsl:text>
    <xsl:value-of select="environment/@nunit-version"/>
    <xsl:text>  </xsl:text>
    <xsl:value-of select="@date"/>
    <xsl:text>  </xsl:text>
    <xsl:value-of select="@time"/>
    <xsl:text>&#xD;&#xA;&#xD;&#xA;</xsl:text>
    
    <xsl:text>Runtime Environment -&#xD;&#xA;</xsl:text>
    <xsl:text>   OS Version: </xsl:text>
    <xsl:value-of select="environment/@os-version"/>
    <xsl:text>&#xD;&#xA;</xsl:text>
    <xsl:text>  CLR Version: </xsl:text>
    <xsl:value-of select="environment/@clr-version"/>
    <xsl:text>&#xD;&#xA;&#xD;&#xA;</xsl:text>

    <xsl:text>Tests run: </xsl:text>
    <xsl:value-of select="@total"/>
    <xsl:choose>
      <xsl:when test ="substring(environment/@nunit-version,1,3)>='2.5'">
        <xsl:text>, Errors: </xsl:text>
        <xsl:value-of select="@errors"/>
        <xsl:text>, Failures: </xsl:text>
        <xsl:value-of select="@failures"/>
        <xsl:if test="@inconclusive">
          <!-- Introduced in 2.5.1 -->
          <xsl:text>, Inconclusive: </xsl:text>
          <xsl:value-of select="@inconclusive"/>
        </xsl:if>
        <xsl:text>, Time: </xsl:text>
        <xsl:value-of select="test-suite/@time"/>
        <xsl:text> seconds&#xD;&#xA;</xsl:text>
        <xsl:text>  Not run: </xsl:text>
        <xsl:value-of select="@not-run"/>
        <xsl:text>, Invalid: </xsl:text>
        <xsl:value-of select="@invalid"/>
        <xsl:text>, Ignored: </xsl:text>
        <xsl:value-of select="@ignored"/>
        <xsl:text>, Skipped: </xsl:text>
        <xsl:value-of select="@skipped"/>
        <xsl:text>&#xD;&#xA;</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>, Failures: </xsl:text>
        <xsl:value-of select="@failures"/>
        <xsl:text>, Not run: </xsl:text>
        <xsl:value-of select="@not-run"/>
        <xsl:text>, Time: </xsl:text>
        <xsl:value-of select="test-suite/@time"/>
        <xsl:text> seconds&#xD;&#xA;</xsl:text>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:text>&#xD;&#xA;</xsl:text>

    <xsl:if test="//test-case[failure]">
      <xsl:text>Failures:&#xD;&#xA;</xsl:text>
    </xsl:if>
    <xsl:apply-templates select="//test-case[failure]"/>
    <xsl:if test="//test-case[@executed='False']">
      <xsl:text>Tests not run:&#xD;&#xA;</xsl:text>
    </xsl:if>
    <xsl:apply-templates select="//test-case[@executed='False']"/>
    <xsl:text disable-output-escaping='yes'>&#xD;&#xA;</xsl:text>
  </xsl:template>

  <xsl:template match="test-case">
    <xsl:value-of select="position()"/>
    <xsl:text>) </xsl:text>
    <xsl:value-of select="@name"/>
    <xsl:text> : </xsl:text>
    <xsl:value-of select="child::node()/message"/>
    <xsl:text disable-output-escaping='yes'>&#xD;&#xA;</xsl:text>
    <xsl:if test="failure">
      <xsl:value-of select="failure/stack-trace"/>
      <xsl:text>&#xD;&#xA;</xsl:text>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
