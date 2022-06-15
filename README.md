# gmcstonedesafio

#Como executar

Este exerc�cio foi desenvolvido em C# vers�o .Net 5.0 
Para compilar e executar tem que ter instalado .NET Runtime 5.0 (ou maior)
(https://dotnet.microsoft.com/en-us/download/dotnet/5.0)

Para executar, ap�s feito o dowload do resposit�rio 
basta alterar o diretorio para /DesafioFaturamento
que contem o arquivo DesafioFaturamento.csproj

E executar o comando
>dotnet run [drive:][path]nomeDoArquivo

Parametros entre colchetes s�o opcionais


Uma pequena funcionalidade que utilizei na constru��o para me ajudar nos testes, 
� rodar o comando da forma
>dotnet run true

Assim o programa utiliza como arquivo de entrada o csv encontrado na pasta 
DesafioFaturamento/data, ele cont�m os mesmos dados de coleta informados na 
especifica��o, e valida com a sa�da esperada.


# Decis�es ao realizar e documenta��o

.Net se baseia na linguagem do sistema operacional para tratar string como 
numero decimais ('.' para casas decimais, e ',' para separador de milhar, e 
vice-versa) ent�o usei uma configuracao global para sempre tratar '.' para casas 
decimais, indepentende do SO, metodos para esse tratamento estao na classe 
FaturamentoUtils

.Net possui alguns m�todos diferentes para realizar o arredondamento de valores, 
eu utilizei o MidpointRounding.AwayFromZero, ou seja, vai arredondar para o 
numero mais perto, quando esta no meio do caminho entre dois numeros arredonda 
para o n�mero mais distante de zero.
Ex 3,234 arredonda para 3,23; 3,235 arredonda para 3,24; 3,236 arredonda para 
3,24.

O arredondamento � realizado na leitura dos valores, pelo que entendi que era a 
especifica��o dos requisitos "Todos os pre�os ter�o no m�ximo duas casas 
decimais, arredondados para a casa decimal mais pr�xima" e n�o somente ap�s as 
totaliza��es serem realizadas.


Linhas que falharam na valida��o s�o ignoradas para o totalizar o valor da nota, 
com exce��o dos casos em que a valida��o depende da combina��o de linhas, e n�o 
teria uma maneira segura de garantir a preced�ncia, nesse caso todas as entradas 
da maquina s�o ignoradas.

Estes casos s�o:
- N�o podem existir duas datas iniciais iguais para um mesmo id.
- entre o per�odo de suas datas.
- Uma maquininha s� poder� ter um evento de Desativa��o ap�s uma
Ativa��o.

Valida��es tanto dos comandos de entrada, das linhas e da consist�ncia dos 
eventos para uma maquina s�o realizadas na classe FaturamentoValidator e todo o 
calculo e regras de neg�cio associadas na classe FaturamentoCalculator.

N�o foi realizada a atividade b�nus.