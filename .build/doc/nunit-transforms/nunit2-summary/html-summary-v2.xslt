<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method='text'/>

  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="test-results">
    <xsl:text>&lt;b&gt;</xsl:text>
    <xsl:value-of select="@name"/>
    <xsl:text>&lt;/b&gt;&lt;br&gt;&lt;br&gt;&#xD;&#xA;&#xD;&#xA;</xsl:text>

    <xsl:text>&lt;b&gt;NUnit Version:&lt;/b&gt; </xsl:text>
    <xsl:value-of select="environment/@nunit-version"/>
    <xsl:text>&amp;nbsp;&amp;nbsp;&amp;nbsp;&lt;b&gt;Date:&lt;/b&gt; </xsl:text>
    <xsl:value-of select="@date"/>
    <xsl:text>&amp;nbsp;&amp;nbsp;&amp;nbsp;&lt;b&gt;Time:&lt;/b&gt; </xsl:text>
    <xsl:value-of select="@time"/>
    <xsl:text>&lt;br&gt;&lt;br&gt;&#xD;&#xA;&#xD;&#xA;</xsl:text>

    <xsl:text>&lt;b&gt;Runtime Environment -&lt;/b&gt;&lt;br&gt;&#xD;&#xA;</xsl:text>
    <xsl:text>&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&lt;b&gt;OS Version:&lt;/b&gt; </xsl:text>
    <xsl:value-of select="environment/@os-version"/>
    <xsl:text>&lt;br&gt;&#xD;&#xA;</xsl:text>
    <xsl:text>&amp;nbsp;&amp;nbsp;&amp;nbsp;&lt;b&gt;CLR Version:&lt;/b&gt; </xsl:text>
    <xsl:value-of select="environment/@clr-version"/>
    <xsl:text>&lt;br&gt;&lt;br&gt;&#xD;&#xA;&#xD;&#xA;</xsl:text>

    <xsl:text>&lt;b&gt;Tests run: </xsl:text>
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
        <xsl:text> seconds&lt;br&gt;</xsl:text>
        <xsl:text>&amp;nbsp;&amp;nbsp;&amp;nbsp;Not run: </xsl:text>
        <xsl:value-of select="@not-run"/>
        <xsl:text>, Invalid: </xsl:text>
        <xsl:value-of select="@invalid"/>
        <xsl:text>, Ignored: </xsl:text>
        <xsl:value-of select="@ignored"/>
        <xsl:text>, Skipped: </xsl:text>
        <xsl:value-of select="@skipped"/>
        <xsl:text>&lt;/b&gt;&lt;br&gt;&lt;br&gt;&#xD;&#xA;</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>, Failures: </xsl:text>
        <xsl:value-of select="@failures"/>
        <xsl:text>, Not run: </xsl:text>
        <xsl:value-of select="@not-run"/>
        <xsl:text>, Time: </xsl:text>
        <xsl:value-of select="test-suite/@time"/>
        <xsl:text> seconds&lt;/b&gt;&lt;br&gt;&lt;br&gt;</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text>
</xsl:text>
  </xsl:template>

</xsl:stylesheet>