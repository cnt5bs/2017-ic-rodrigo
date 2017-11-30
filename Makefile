
Relatorio-IC.html: Projeto*.ad
	asciidoctor -o "$@" "$<"

Relatorio-IC.pdf: Projeto*.ad
	asciidoctor-pdf -o "$@" "$<"
