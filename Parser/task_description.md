#### File format
All parameter and section names are strings without spaces, consisting of Latin characters, numbers and underscores.

#### Keys (properties)
The basic element contained in an INI file is the key or property. Every key has a name and a value, delimited by an equals sign (=). The name appears to the left of the equals sign.

#### Parameter values can be of one of the following types:
- integer
- real
- string: no spaces, but unlike the parameter name may contain dots (.)

#### Sections
Keys may, but need not, be grouped into arbitrarily named sections. The section name appears on a line by itself, in square brackets ([ and ]). All keys after the section declaration are associated with that section. There is no explicit "end of section" delimiter; sections end at the next section declaration, or at the end of the file. Sections cannot be nested.

In this implementation, the absence of a section is considered an erroneous INI file representation.

#### Case sensitivity
In this implementation, the names of sections and parameters are case sensitive.

#### Comments
Semicolons (;) at the beginning of the line indicate a comment. Comment lines are ignored.
