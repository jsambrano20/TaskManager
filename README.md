# TaskManager API

O **TaskManager API** é uma aplicação backend projetada para gerenciar tarefas e projetos. A API fornece endpoints para autenticação, auditoria, geração de relatórios e mais. Este projeto utiliza tecnologias modernas como ASP.NET Core, PostgreSQL e Docker para garantir facilidade de execução e escalabilidade.

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter os seguintes itens instalados em sua máquina:

- **Docker**: [Instalar Docker](https://www.docker.com/products/docker-desktop)
- **Git**: [Instalar Git](https://git-scm.com/)

## 🚀 Executando o Projeto no Docker

### 1. Clonar o Repositório

Clone o repositório do projeto em sua máquina local:

```bash
git clone https://github.com/jsambrano20/TaskManager.git
cd taskmanager
```

### 2. Iniciar o Docker Compose

Execute o seguinte comando para iniciar os contêineres:

```bash
docker-compose up --build
```

Este comando fará o seguinte:
- Criará e iniciará o contêiner do PostgreSQL.
- Construirá e iniciará o contêiner da API.

### 3. Verificar os Logs

Você pode verificar os logs dos contêineres para garantir que tudo esteja funcionando corretamente:

```bash
docker-compose logs -f
```

### 4. Acessar a API

Após a inicialização, a API estará disponível na porta `8080` (HTTP). Você pode testar os endpoints usando ferramentas como [Swagger](http://localhost:8080/swagger/index.html).

#### Endpoints Principais:
- **Autenticação**: `/api/auth/login`
- **Relatórios**: `/api/reports/export/pdf`, `/api/reports/export/excel`

---

## 🛠️ Comandos Úteis

| Comando                          | Descrição                                      |
|----------------------------------|------------------------------------------------|
| `docker-compose up --build`      | Inicia os contêineres (constrói se necessário). |
| `docker-compose down`            | Para e remove os contêineres.                  |
| `docker-compose logs -f`         | Exibe logs em tempo real.                      |

---

## 📂 Estrutura do Projeto

```
taskmanager-api/
├── src/
│   ├── TaskManager.API/          # Camada de API (Controllers, Middleware)
│   ├── TaskManager.Application/  # Lógica de negócios e serviços
│   ├── TaskManager.Domain/       # Entidades, Interfaces e Enums
│   ├── TaskManager.Infrastructure/ # Repositórios e Persistência
├── docker-compose.yml            # Configuração do Docker Compose
├── Dockerfile                    # Configuração do Docker para a API
├── README.md                     # Documentação do projeto
└── .env                          # Variáveis de ambiente
```

---

## 🐳 Docker Compose

O arquivo `docker-compose.yml` define dois serviços:

1. **db**: Contêiner do PostgreSQL.
2. **api**: Contêiner da API.

#### Conteúdo do `docker-compose.yml`:

```yaml
version: '3.8'

services:
  taskmanager.api:
    image: ${DOCKER_REGISTRY-}taskmanagerapi
    build:
      context: .
      dockerfile: src/TaskManager.API/Dockerfile
    container_name: taskmanager-api
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=db;Database=task-manager-db;Username=admin;Password=senha123;
    depends_on:
      - db

  db:
    image: postgres:14-alpine
    container_name: taskmanager-db
    environment:
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: senha123
      POSTGRES_DB: task-manager-db 
    ports:
      - "5433:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    restart: always

volumes:
  postgres_data:
```
#### Refinamento:

Teremos CRUD de usuário para controle de acesso?
Será necessário implementar um CRUD completo para a gestão de usuários, permitindo a criação, leitura, atualização e exclusão de registros de usuários no sistema? Isso será fundamental para garantir um controle adequado de permissões e acesso ao sistema.

Apenas a API de relatórios teria autenticação por role?
A API de relatórios será a única parte do sistema que terá autenticação baseada em roles, ou devemos aplicar autenticação e autorização baseada em roles para outras partes do sistema também?

Melhoria no controle de ações por usuário baseado em roles?
Poderíamos considerar uma abordagem mais detalhada no controle de acesso? Por exemplo, a definição de roles mais específicas, permitindo um controle refinado sobre quais ações cada usuário pode realizar no sistema, em vez de aplicar permissões mais gerais.

Todo ou qualquer usuário pode alterar projetos, incluir tarefas e comentários?
Todos os usuários terão permissão para editar projetos e adicionar tarefas e comentários? Ou será necessário restringir algumas dessas ações a tipos específicos de usuários?

Endpoint para consumir o projeto do usuário logado e filtros personalizados:
Criei um endpoint específico para consumir o projeto do usuário logado, pensando na questão da personalização da experiência. Seria interessante também adicionar endpoints com filtros personalizados, para que o sistema não apenas retorne todos os registros, mas permita que o usuário filtre os dados com base em suas permissões e preferências.

Endpoints e detalhes não uniformes:
Verifiquei que alguns endpoints foram solicitados sem muitos detalhes e outros com falta de especificações. Seria bom revisar os detalhes e garantir que todos os endpoints tenham as informações necessárias para uma implementação consistente e eficiente.

---
#### Melhorias:

Pontos de Melhoria no Projeto:
CRUD com filtros:
Um ponto importante seria a implementação de filtros mais avançados nas operações de CRUD, permitindo que os usuários realizem buscas refinadas com base em diferentes parâmetros. Isso melhoraria a usabilidade do sistema e tornaria as APIs mais flexíveis.

Criação de CRUD para Usuário e Autenticação Externa:
Como o sistema será utilizado externamente, seria interessante implementar um CRUD completo para a gestão de usuários. Além disso, a autenticação externa via Google ou Microsoft poderia ser adicionada para simplificar o processo de login e melhorar a segurança.

Uso de banco de dados performáticos e cache:
Para garantir a performance, seria bom migrar para um banco de dados mais performático, como PostgreSQL, e considerar a implementação de cache (por exemplo, Redis) para melhorar o tempo de resposta em consultas frequentes.

Relatórios automáticos e armazenamento em blob:
Implementar jobs semanais e mensais para a geração automática de relatórios, com o salvamento desses relatórios em um storage blob (por exemplo, Azure Blob Storage) para uma recuperação eficiente e segura.

Arquitetura DDD:
Considerando que o sistema é mais enxuto, a adoção da arquitetura DDD (Domain-Driven Design) seria vantajosa. Isso facilitará a implementação de novas funcionalidades e a manutenção do sistema a longo prazo, além de garantir uma estrutura mais modular e coesa.
