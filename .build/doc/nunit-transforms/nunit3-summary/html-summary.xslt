<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output omit-xml-declaration="yes" indent="yes" encoding="UTF-8" />
  <xsl:template match="/">
    <style type="text/css">

      .strong {
      font-weight: bold;
      }

      .smllabel {
      width:110px;
      font-weight: bold;
      }

      td.left {
      text-align: left;
      }

      td.right {
      text-align: right;
      }

      li {
      font-family: monospace
      }
      .collapsed
      {
      border:0;
      border-collapse: collapse;
      }

    </style>
    <xsl:apply-templates/>

  </xsl:template>

  <xsl:template match="test-run">

    <!-- Runtime Environment -->
    <h4>Runtime Environment</h4>

    <table id="runtime">
      <tr>
        <td class="smllabel right">OS Version:</td>
        <td class="left">
          <xsl:value-of select="test-suite/environment/@os-version[1]"/>
        </td>
      </tr>
      <tr>
        <td class="smllabel right">CLR Version:</td>
        <td class="left">
          <xsl:value-of select="@clr-version"/>
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
        </td>
      </tr>
      <tr>
        <td class="smllabel right">NUnit Version:</td>
        <td class="left">
          <xsl:value-of select="@engine-version"/>
        </td>
      </tr>
    </table>

    <h4>Test Run Summary</h4>
    <table id="summary" class="collapsed">
      <tr>
        <td class="smllabel right">Overall result:</td>
        <td class="left">
          <xsl:value-of select="@result"/>
        </td>
      </tr>
      <tr>
        <td class="smllabel right">Test Count:</td>
        <td class="left">
          <xsl:value-of select="@total"/>, Passed: <xsl:value-of select="@passed"/>, Failed: <xsl:value-of select="@failed"/>, Inconclusive: <xsl:value-of select="@inconclusive"/>, Skipped: <xsl:value-of select="@skipped"/>
        </td>
      </tr>

      <!-- [Optional] - Failed Test Summary -->
      <xsl:if test="@failed > 0">
        <xsl:variable name="failedTotal" select="count(//test-case[@result='Failed' and not(@label)])" />
        <xsl:variable name="errorsTotal" select="count(//test-case[@result='Failed' and @label='Error'])" />
        <xsl:variable name="invalidTotal" select="count(//test-case[@result='Failed' and @label='Invalid'])" />
        <tr>
          <td class="smllabel right">Failed Tests: </td>
          <td class="left">
            Failures: <xsl:value-of select="$failedTotal"/>, Errors: <xsl:value-of select="$errorsTotal"/>, Invalid: <xsl:value-of select="$invalidTotal"/>
          </td>
        </tr>
      </xsl:if>

      <!-- [Optional] - Skipped Test Summary -->
      <xsl:if test="@skipped > 0">
        <xsl:variable name="ignoredTotal" select="count(//test-case[@result='Skipped' and @label='Ignored'])" />
        <xsl:variable name="explicitTotal" select="count(//test-case[@result='Skipped' and @label='Explicit'])" />
        <xsl:variable name="otherTotal" select="count(//test-case[@result='Skipped' and not(@label='Explicit' or @label='Ignored')])" />
        <tr>
          <td class="smllabel right">Skipped Tests: </td>
          <td class="left">
            Ignored: <xsl:value-of select="$ignoredTotal"/>, Explicit: <xsl:value-of select="$explicitTotal"/>, Other: <xsl:value-of select="$otherTotal"/>
          </td>
        </tr>
      </xsl:if>

      <!-- Times -->
      <tr>
        <td class="smllabel right">Start time: </td>
        <td class="left">
          <xsl:value-of select="@start-time"/>
        </td>
      </tr>
      <tr>
        <td class="smllabel right">End time: </td>
        <td class="left">
          <xsl:value-of select="@end-time"/>
        </td>
      </tr>
      <tr>
        <td class="smllabel right">Duration: </td>
        <td class="left">
          <xsl:value-of select="format-number(@duration,'0.000')"/> seconds
        </td>
      </tr>
    </table>
   
  </xsl:template>

</xsl:stylesheet>
