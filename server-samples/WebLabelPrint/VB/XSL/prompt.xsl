<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">

  <xsl:template name="replace">
    <xsl:param name="string"/>
    <xsl:choose>
      <xsl:when test="contains($string,'&#10;')">
        <xsl:value-of select="substring-before($string,'&#10;')"
   disable-output-escaping="yes"/>
        <br/>
        <xsl:call-template name="replace">
          <xsl:with-param name="string"
    select="substring-after($string,'&#10;')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string" disable-output-escaping="yes"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:output method="html" />
  <xsl:template match="/">
    <script type="text/javascript" src="./javascript/Validation.js" language="javascript"></script>
    <div
      style="
          width: {PROMPTSCREEN/BACKGROUND/DIMENSION/WIDTH}px;
          height: {PROMPTSCREEN/BACKGROUND/DIMENSION/HEIGHT}px;
          border-size: inherit;
          position: relative;
        ">
      <xsl:apply-templates/>
    </div>
    
  </xsl:template>

  <xsl:template match="PROMPTSCREEN/BACKGROUND">
    <!--do nothing, we already created the background div-->
  </xsl:template>
  <xsl:template match="PROMPTSCREEN/GROUPBOX">
    <div 
      class="PromptLine" 
      style="
    position: absolute; 
    top: {POSITION/Y+(LABEL_HEIGHT div 2)}px; 
    left: {POSITION/X}px;
    width: {DIMENSION/WIDTH}px;
    height: {DIMENSION/HEIGHT+-(LABEL_HEIGHT div 2)}px;
    border-size:  inherit;
  ">
    </div>
    <div 
      class="PromptBackground" 
      style=" 
    color: {FONT/TEXT_COLOR}; 
    position: absolute; 
    top: {POSITION/Y}px; 
    left: {POSITION/X+LABEL_HEIGHT}px;
    font-family: {FONT/TYPEFACE};
    font-size: {FONT/POINT_SIZE}pt;
    text-decoration: {FONT/DECORATION};
    font-style: '{FONT/STYLE}';
    font-weight: {FONT/WEIGHT};
    text-align: {FONT/ALIGNMENT};
    direction: {FONT/READ_ORDER};
  ">
      <xsl:value-of select="TEXT" />
    </div>
  </xsl:template>
  <xsl:template match="PROMPTSCREEN/VERTICAL_LINE">
    <div 
      class="PromptLine" 
      style="
    position: absolute; 
    top: {POSITION/Y - (DIMENSION/WIDTH div 2)}px; 
    left: {POSITION/X + (DIMENSION/WIDTH div 2)}px;
    width: {DIMENSION/HEIGHT}px;
    height: {DIMENSION/WIDTH}px;
    font-size:0;
    line-height:0;
    border-top: 0px;
    border-right: 0px;
    border-bottom: 0px;
    padding: 0px;
  "></div>
  </xsl:template>
  <xsl:template match="PROMPTSCREEN/HORIZONTAL_LINE">
    <div 
      class="PromptLine" 
      style="
    position: absolute; 
    top: {POSITION/Y}px; 
    left: {POSITION/X}px;
    width: {DIMENSION/WIDTH}px;
    height: {DIMENSION/HEIGHT}px;
    font-size: 0;
    line-height: 0;
    border-left: 0px;
    border-right: 0px;
    border-bottom: 0px;
    padding: 0px;
  "></div>
  </xsl:template>
  <xsl:template match="PROMPTSCREEN/TEXTBOX">
    <div style=" 
    color: {FONT/TEXT_COLOR}; 
    position: absolute; 
    top: {POSITION/Y}px; 
    left: {POSITION/X}px;
    font-family: {FONT/TYPEFACE};
    font-size: {FONT/POINT_SIZE}pt;
    width: {DIMENSION/WIDTH}px;
    height: {DIMENSION/HEIGHT}px;
    word-wrap: break-word;
    text-decoration: {FONT/DECORATION};
    font-style: '{FONT/STYLE}';
    font-weight: {FONT/WEIGHT};
    text-align: {FONT/ALIGNMENT};
    direction: {FONT/READ_ORDER};
    display: block;
  ">
      <xsl:call-template name="replace">
        <xsl:with-param name="string" select="TEXT"/>
      </xsl:call-template>

    </div>
  </xsl:template>
  <xsl:template match="PROMPTSCREEN/DROPDOWN">
    <asp:DropDownList
      runat ="server"
      style=" 
      color: {FONT/TEXT_COLOR}; 
      position: absolute; 
      top: {POSITION/Y}px; 
      left: {POSITION/X}px;
      font-family: {FONT/TYPEFACE};
      font-size: {FONT/POINT_SIZE}pt;
      width: {DIMENSION/WIDTH}px;
      height: {DIMENSION/HEIGHT}px;
      word-wrap: break-word;
      text-decoration: {FONT/DECORATION};
      font-style: '{FONT/STYLE}';
      font-weight: {FONT/WEIGHT};
      text-align: {FONT/ALIGNMENT};
      direction: {FONT/READ_ORDER};
      display: block;
    "
    >
      <xsl:attribute name="ID">
        <xsl:value-of select="translate(NAME,' ','_')"/>
      </xsl:attribute>

      <xsl:for-each select="LIST_ITEMS">
        <asp:ListItem>
          <xsl:if test="SELECTED='TRUE'">
            <xsl:attribute name="Selected">
              <xsl:value-of select="'true'" />
            </xsl:attribute>
          </xsl:if>
          <xsl:call-template name="replace">
            <xsl:with-param name="string" select="LIST_ITEM"/>
          </xsl:call-template>
        </asp:ListItem>
      </xsl:for-each>
    </asp:DropDownList>

  </xsl:template>
  <xsl:template match="PROMPTSCREEN/LISTBOX">
    <asp:ListBox
      runat ="server"
      rows="{NUM_LIST_ITEMS}"
      style="
      color: {FONT/TEXT_COLOR};
      position: absolute;
      top: {POSITION/Y}px;
      left: {POSITION/X}px;
      font-family: {FONT/TYPEFACE};
      font-size: {FONT/POINT_SIZE}pt;
      width: {DIMENSION/WIDTH}px;
      word-wrap: break-word;
      text-decoration: {FONT/DECORATION};
      font-style: '{FONT/STYLE}';
      font-weight: {FONT/WEIGHT};
      text-align: {FONT/ALIGNMENT};
      direction: {FONT/READ_ORDER};
      display: block;
    "
  >
      <xsl:attribute name="ID">
        <xsl:value-of select="translate(NAME,' ','_')"/>
      </xsl:attribute>
      <xsl:for-each select="LIST_ITEMS">
        <asp:ListItem>
          <xsl:if test="SELECTED='TRUE'">
            <xsl:attribute name="Selected">
              <xsl:value-of select="'true'" />
            </xsl:attribute>
          </xsl:if>
          <xsl:call-template name="replace">
            <xsl:with-param name="string" select="LIST_ITEM"/>
          </xsl:call-template>
        </asp:ListItem>
      </xsl:for-each>
    </asp:ListBox>
  </xsl:template>
  <xsl:template match="PROMPTSCREEN/EDITBOX">
    <div style="
      position: absolute; 
      top: {POSITION/Y}px; 
      left: {POSITION/X}px;
      width: {DIMENSION/WIDTH}px;
      height: {DIMENSION/HEIGHT}px;
    ">
      <asp:TextBox
        runat ="server"
        style="
        color: {FONT/TEXT_COLOR}; 
        width: {DIMENSION/WIDTH+ -2}px;
        height: {DIMENSION/HEIGHT+ -2}px;
        font-family: {FONT/TYPEFACE};
        font-size: {FONT/POINT_SIZE}pt;
        text-decoration: {FONT/DECORATION};
        font-style: '{FONT/STYLE}';
        font-weight: {FONT/WEIGHT};
        text-align: {FONT/ALIGNMENT};
        direction: {FONT/READ_ORDER};
        margin: 0px;
        padding: 0px;
        "
      >
        <xsl:attribute name="ID">
          <xsl:value-of select="translate(NAME,' ','_')"/>
        </xsl:attribute>
        <xsl:attribute name="rows">
          <xsl:value-of select="NUM_LINES"/>
        </xsl:attribute>
        <xsl:if test="NUM_LINES &gt; 1">
          <xsl:attribute name="TextMode">
            <xsl:value-of select="'MultiLine'" />
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="MAX_CHARS &gt; 0">
          <xsl:attribute name="MaxLength">
            <xsl:value-of select="MAX_CHARS" />
          </xsl:attribute>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="DATASOURCE = 'DatabaseField'">
            <xsl:attribute name="Enabled">
              <xsl:value-of select="'false'" />
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="DATASOURCE = 'VisualBasicScript'">
            <xsl:attribute name="Enabled">
              <xsl:value-of select="'false'" />
            </xsl:attribute>
          </xsl:when>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="VALIDATION = 'NUMERIC'">
            <xsl:attribute name="onkeypress">
              <xsl:value-of select="'return IsNumeric(GetCharcode(event))'" />
            </xsl:attribute>
            <xsl:attribute name="onchange">
              <xsl:value-of select="concat(concat('return IsNumericOnChange(this,&quot;', CASE), '&quot;)')" />
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="VALIDATION = 'ALPHA'">
            <xsl:attribute name="onkeypress">
              <xsl:value-of select="'return IsAlpha(GetCharcode(event))'" />
            </xsl:attribute>
            <xsl:attribute name="onchange">
              <xsl:value-of select="concat(concat('return IsAlphaOnChange(this,&quot;', CASE), '&quot;)')" />
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="VALIDATION = 'ALPHANUMERIC'">
            <xsl:attribute name="onkeypress">
              <xsl:value-of select="'return IsAlphaNumeric(GetCharcode(event))'" />
            </xsl:attribute>
            <xsl:attribute name="onchange">
              <xsl:value-of select="concat(concat('return IsAlphaNumericOnChange(this,&quot;', CASE), '&quot;)')" />
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="VALIDATION = 'HEX'">
            <xsl:attribute name="onkeypress">
              <xsl:value-of select="'return IsHex(GetCharcode(event))'" />
            </xsl:attribute>
            <xsl:attribute name="onchange">
              <xsl:value-of select="concat(concat('return IsHexOnChange(this,&quot;', CASE), '&quot;)')" />
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="VALIDATION = 'CURRENCY'">
            <xsl:attribute name="onkeypress">
              <xsl:value-of select="'return IsCurrency(GetCharcode(event))'" />
            </xsl:attribute>
            <xsl:attribute name="onchange">
              <xsl:value-of select="concat(concat('return IsCurrencyOnChange(this,&quot;', CASE), '&quot;)')" />
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="VALIDATION = 'CUSTOM'">
            <xsl:attribute name="onkeypress">
              <xsl:value-of select="concat(concat('return IsCustom(GetCharcode(event),&quot;', CUSTOM_CHARS), '&quot;)')" />
            </xsl:attribute>
            <xsl:attribute name="onchange">
              <xsl:value-of select="concat(concat(concat(concat('return IsCustomOnChange(this,&quot;', CUSTOM_CHARS),'&quot;,&quot;'),CASE),'&quot;)')" />
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="VALIDATION = 'ALL' or VALIDATION = 'INPUT_MASK'">
            <xsl:attribute name="onchange">
              <xsl:value-of select="concat(concat('return IsAllOnChange(this,&quot;', CASE), '&quot;)')" />
            </xsl:attribute>
          </xsl:when>
        </xsl:choose>
        <xsl:value-of select="TEXT" />
      </asp:TextBox>
    </div>
  </xsl:template>

</xsl:stylesheet>