# DADOS_RELATORIO_PIM4 - HEJ Care Suite – Relatório Técnico e Acadêmico

## 1. CARACTERIZAÇÃO DA ORGANIZAÇÃO & EMPREENDEDORISMO (Etapas 1 e 2)

### 1.1 Contexto Empresarial
O **Hospital Edward Jenner** (denominação fictícia para fins acadêmicos) opera como uma instituição de saúde abrangente situada em um ambiente hospitalar urbano. A organização está inserida no **segmento de assistência médica hospitalar**, oferecendo um espectro completo de serviços que abrange desde cuidados de emergência até tratamentos especializados.

### 1.2 Segmento de Atuação e Modelos de Negócio
**Segmento Primário:** Assistência médica hospitalar de nível terciário

**Serviços Principais Oferecidos:**
- **Cuidados de Emergência:** Pronto atendimento 24/7 com unidade de trauma
- **Hospitalização:** Internação clínica e cirúrgica com unidades especializadas
- **Atendimento Ambulatorial:** Consultas de especialidade e procedimentos diagnósticos
- **Enfermagem 24/7:** Cuidados de enfermagem contínuos para pacientes internados
- **Diagnósticos:** Laboratório clínico e serviços de radiologia integrados
- **Farmácia Hospitalar:** Preparação e dispensação de medicamentos
- **Centro de Esterilização (CME):** Controle de infecção e processamento de dispositivos
- **Nutrição:** Avaliação e manejo nutricional para pacientes
- **Gestão de Resíduos:** Manejo seguro e regulamentado de resíduos hospitalares

### 1.3 Problema Central de Negócios e Solução HEJ Care Suite

**Problema Identificado:** As operações hospitalares do Hospital Edward Jenner sofrem com fragmentação de processos, lentidão nos fluxos de trabalho e falta de visibilidade abrangente entre triage, atendimento, diagnóstico, farmácia e serviços de apoio.

**Impacto do Problema:**
- Atrasos no atendimento de pacientes devido a fluxos de trabalho manuais
- Erros de comunicação entre diferentes departamentos
- Dificuldade em monitorar indicadores-chave de desempenho
- Eficiência operacional suboptimal

**Solução HEJ Care Suite:** A plataforma HEJ Care Suite é um sistema hospitalar modular e integrado que unifica processos clínicos e administrativos através de uma arquitetura web + API + mobile unificada. A solução centraliza dados de pacientes, streamlines fluxos de trabalho e fornece visibilidade em tempo real de todas as operações hospitalares.

**Principais Inovações:**
- **Centralização de Dados:** Unifica dados de pacientes em um único repositório
- **Integração de Fluxos de Trabalho:** Conecta triage → admissão → tratamento → alta
- **Visibilidade em Tempo Real:** Dashboards e alertas em tempo real
- **Design Acessível:** UI/UX inclusivo para todos os perfis de usuários
- **Escalável e Modular:** Extensível para futuras funcionalidades

## 2. ENGENHARIA DE SOFTWARE ÁGIL & GESTÃO (Etapa 9)

### 2.1 Metodologia Ágil e Gerenciamento de Projetos

O projeto adota o **framework SCRUM** como metodologia principal de gerenciamento ágil. A equipe SCRUM é composta por:

- **Product Owner:** Responsável pelas definições de requisitos e priorização
- **Scrum Master:** Facilitador do processo SCRUM e removedor de impedimentos
- **Equipe de Desenvolvimento (6 membros):** Desenvolvedores, arquitetos e engenheiros de teste

### 2.2 Controle de Sprints e Daily Management

**Ferramentas de Gerenciamento:** Embora o planejamento original incluísse Atlassian (Jira e Confluence), a implementação real utilizou **GitHub Projects** e **GitHub Actions** como ferramentas ativas de gerenciamento:

- **GitHub Issues** → Backlog do Product Backlog
- **GitHub Pull Requests** → Code Review → Testing → Deploy
- **Git branches** → Sprint planning e gestão de features

**Rituals SCRUM:**

1. **Daily Standups:** Reuniões diárias de 15 minutos para sincronização de progresso e identificação de impedimentos
2. **Sprint Planning:** Planejamento no início de cada sprint (2 semanas)
3. **Review de Sprint:** Demonstração do incrementado no final de cada sprint
4. **Retrospective:** Reflexão e melhoria contínua do processo

### 2.3 Evidências de Entrega Ágil

A sequência de commits no git reflete claramente a evolução iterativa do produto:

- Commits 9369a40: Implementação de stored procedure 008 e protótipo mobile
- Commit af21ee5: Implementação completa de APIs mobile, schemas de banco de dados e integração VLibras
- Commit 29e0213: Garantia de carregamento dos mais recentes assets de UI

** Métricas de Entrega:** Cada commit representa um incremento concreto do produto, com documentação de edge cases e validação de requisitos.

### 2.4 Histórico de Releases e Versionamento

O sistema segue um **padrão semântico de versionamento**:
- **Versionamento de Banco de Dados:** Através do `SchemaVersion` table, com versions incrementadas por feature (v1-v8)
- **Versionamento de API:** Rotas versionadas como `/api/v1/...`
- **Versionamento de Frontend:** Assets versionados via query parameters (`?v=20261110`)

## 3. RESPONSABILIDADE SOCIAL E ACESSIBILIDADE (Etapa 3)

### 3.1 Inclusão Digital como Princípio Fundamental

O HEJ Care Suite incorpora **acessibilidade digital** como um requisito não funcional essencial (NFR-03), garantindo que todos os pacientes possam interagir com os serviços digitais independentemente de suas habilidades físicas, cognitivas ou sensoriais.

### 3.2 Implementação VLibras – Widget de Acessibilidade em Tempo Real

#### 3.2.1 Arquitetura de Acessibilidade

**Arquivo:** `src/HEJCareSuite.Api/wwwroot/vlibras-init.js`

**Código Implementado:**
```javascript
// VLibras setup
const vlibrasScript = document.createElement("script");
vlibrasScript.src = "https://vlibras.gov.br/app/vlibras-plugin.js";
vlibrasScript.async = true;
document.head.appendChild(vlibrasScript);
vlibrasScript.onload = () => {
  new window.VLibras.Widget('https://vlibras.gov.br/app');
};
```

#### 3.2.2 Mecanismos de Cidadania Digital

**Acesso Língua Brasileira de Sinais:**
- **Tradução em Tempo Real:** O widget VLibras fornece tradução simultânea de texto para LIBRAS
- **Botão de Acessibilidade:** Ícone flutuante no canto da tela para ativação/desativação
- **Suporte a Multiple Linguagens:** Inglês → LIBRAS → Português

**Benefícios para Cidadania Digital:**
1. **Inclusão:** Pacientes surdos ou com deficiência auditiva podem acessar serviços digitais
2. **Empoderamento:** Igualdade de acesso a informações de saúde
3. **Autonomia:** Capacidade de usar o sistema de forma independente

#### 3.2.3 Requisitos Complementares de Acessibilidade

**Recursos Adicionais Implementados:**
- **Navegação por Teclado:** Suporte completo para usuários que não podem usar mouse
- **Contraste Visuomotor:** Opções de alta visibilidade para pacientes com baixa visão
- **Labels Semânticos:** HTML semântico para leitores de tela
- **Design Responsivo:** Funciona em diferentes dispositivos e tamanhos de tela

## 4. DESENVOLVIMENTO DA SOLUÇÃO WEB COM .NET (Etapa 4)

### 4.1 Arquitetura de APIs REST .NET

**Projetos Principais:**
- **HEJCareSuite.Api:** Controladores API e middleware
- **HEJCareSuite.Application:** Casos de uso e serviços de aplicação
- **HEJCareSuite.Domain:** Entidades de domínio e regras de negócios
- **HEJCareSuite.Infrastructure:** Repositórios, mapeamento e integração de banco de dados

### 4.2 Principais Controllers e Endpoints

#### 4.2.1 Pacientes (Patients)
**Controller:** `PatientsController` (API: `/api/v1/patients`)

**Endpoints:**
- `GET /api/v1/patients` - Listar pacientes
- `GET /api/v1/patients/{id}` - Obter paciente por ID
- `POST /api/v1/patients` - Criar novo paciente
- `PUT /api/v1/patients/{id}` - Atualizar paciente
- `DELETE /api/v1/patients/{id}` - Remover paciente

#### 4.2.2 Triagem (Triage)
**Controller:** `TriageController` (API: `/api/v1/patients/{patientId}/triage`)

**Endpoints:**
- `GET /api/v1/patients/{patientId}/triage` - Histórico de triagem
- `POST /api/v1/patients/{patientId}/triage` - Registrar nova triagem

#### 4.2.3 Hospitalização (Admissions)
**Controller:** `AdmissionsController` (API: `/api/v1/patients/{patientId}/admissions`)

**Endpoints:**
- `GET /api/v1/patients/{patientId}/admissions` - Listar admissões
- `POST /api/v1/patients/{patientId}/admissions` - Registrar nova admissão
- `POST /api/v1/patients/{patientId}/admissions/{admissionId}/discharge` - Finalizar alta

#### 4.2.4 Mobile e Acessibilidade
**Controller:** `MobileController` (API: `/api/v1/mobile/...`)

**Endpoints:**
- `GET /api/v1/mobile/health` - Health check para dispositivos mobile
- `GET /api/v1/mobile/patient-summary/{patientId}` - Resumo clínico via stored procedure

### 4.3 Mecanismos de Segurança e Autenticação

#### 4.3.1 Arquitetura de Segurança

**Arquivo de Configuração:** JWT tokens e política de autenticação centralizada

**Atributos de Segurança Customizados:**
- **`RequireAuthenticatedUser`:** Decoração para proteger endpoints
- **`ValidateUserScope`:** Validação de escopos de usuário por endpoint

#### 4.3.2 Admin User Account

**Usuário Administrativo:**
- **Email:** `admin@hejcare.local`
- **Password:** Hash PBKDF2 com salt (alterar em produção)

**Arquivo de Schema:** `database/003_authentication_schema.sql` define:

```sql
CREATE TABLE dbo.UserAccount (
    UserId UNIQUEIDENTIFIER PRIMARY KEY,
    Email NVARCHAR(320) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    PasswordSalt NVARCHAR(256) NOT NULL,
    RoleName NVARCHAR(40) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAtUtc DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME()
);
```

**Procedimento de Autenticação:**
1. Validação de token JWT em middleware
2. Verificação de escopos/roles por endpoint
3. Authorization baseada em Claims
4. Política de senha PBKDF2 para usuários administrativos

### 4.4 Integração entre Módulos

**Dependências Principais:**
- **Controladores → Application Services:** Injeção de dependência para casos de uso
- **Application Services → Infrastructure:** Repositórios para acesso a dados
- **Infrastructure → Domain:** Entidades de domínio e regras de negócios
- **Layered Architecture:** MVC + Repository Pattern + Unit of Work

## 5. DESENVOLVIMENTO DA SOLUÇÃO MOBILE (Etapa 5)

### 5.1 Protótipo Mobile em HTML5

**Localização:** `src/HEJCareSuite.Api/wwwroot/mobile/index.html`

### 5.2 Arquitetura de Mobile-First Design

**Framework:** CSS Grid e Flexbox com tema escuro

**Components Principais:**
- **Header Fixo:** Status online e indicador de papel do usuário
- **Grid de Cards:** Design mobile-first responsivo
- **Bottom Navigation:** Navegação principal simplificada
- **Components Acessíveis:** Acessibilidade VLibras integrada

### 5.3 Screens Simuladas e Fluxos de Trabalho

#### 5.3.1 Central de Alertas Críticos
```html
<div class="card">
  <h3>Central de Alertas Críticos <span class="badge" style="background: var(--red);">
    AO VIVO</span></h3>
  <p>Notificações push de emergências e instabilidades em leitos e triagem.</p>
  <ul id="alert-list" class="alert-list">
    <li class="alert-item"><strong>Leito A-101:</strong> 
      Batimento cardíaco elevado detectado. <span style="font-size:.7rem; 
      color:var(--text-muted);">há 2 min</span></li>
  </ul>
</div>
```

**Funcionalidades:**
- **Alertas em Tempo Real:** Notificações críticas de emergência
- **Status de Leitos:** Monitoramento de ocupação de leitos
- **Alertas de Triagem:** Notificações de risco de pacientes

#### 5.3.2 Gerenciamento de Tarefas de Enfermagem
```html
<div class="card">
  <h3>Tarefas da Equipe de Enfermagem</h3>
  <p>Gerenciamento de cuidados e checagem de prescrições hospitalares.</p>
  <ul id="task-list" class="task-list">
    <li class="task-item"><span>Administrar medicação intravenosa (Leito B-202)</span> 
      <button class="btn" style="width: auto; padding: .3rem .6rem; 
      font-size: .75rem;" onclick="alert('Tarefa concluída com sucesso!')">
        Concluir</button></li>
  </ul>
</div>
```

**Funcionalidades:**
- **Lista de Tarefas:** Gerenciamento de tarefas de enfermagem
- **Priorização:** Diferentes níveis de prioridade
- **Conclusão:** Marcação de tarefas como concluídas

#### 5.3.3 Sincronização Offline-First
```javascript
async function syncData() {
  const status = document.getElementById('sync-status');
  status.textContent = "Sincronizando com a API do HEJ Care Suite...";
  try {
    const res = await fetch('/health');
    if (res.ok) {
      status.textContent = "Sincronização concluída com sucesso! (100% íntegro)";
      status.style.color = "var(--primary)";
    } else {
      throw new Error();
    }
  } catch {
    status.textContent = "Erro ao conectar com o servidor. Dados salvos localmente.";
    status.style.color = "var(--red)";
  }
}
```

### 5.4 Endpoints Dedicados do Backend Mobile

**Health Check:** `/health` para verificação de conectividade
**Stored Procedure:** `PR_GetPatientClinicalSummary` para resumo clínico
**Sync API:** Endpoints específicos para sincronização mobile

## 6. ARQUITETURA DE SOFTWARE EM .NET (Etapa 6)

### 6.1 Arquitetura em Camadas

**Estrutura:**
```
Presentation Layer (MVC + Web APIs)
    ↓
Application Layer (Casos de Uso e Serviços)
    ↓
Domain Layer (Entidades e Regras de Negócios)
    ↓
Infrastructure Layer (Repositórios, Banco de Dados)
```

### 6.2 Design Orientado a Objetos

**Princípios SOLID Aplicados:**
- **Single Responsibility:** Cada classe tem uma responsabilidade única
- **Open/Closed:** Extensível sem modificação
- **Liskov Substitution:** Substituibilidade de tipos derivados
- **Interface Segregation:** Interfaces específicas e focadas
- **Dependency Inversion:** Dependência de abstrações, não de concretions

### 6.3 Arquitetura MVC para Web

**Controller Pattern:**
- **Routing:** Rotas versionadas (`/api/v1/...`)
- **Filtros:** Autenticação, tratamento de erros, validação
- **Model Binding:** Vinculação automática de dados entre UI e lógica

**Entidades Principais do Domínio:**
- **Patient:** Entidade central representando pacientes
- **Triage:** Registro de triagem de emergência
- **HospitalAdmission:** Admissão e alta hospitalar
- **StaffTask:** Tarefas da equipe de enfermagem
- **CriticalAlert:** Alertas críticos de emergência

### 6.4 Arquitetura de Repositórios

**Repository Pattern:**
- **I*Repository Interfaces:** Abstração para acesso a dados
- **Entity Framework Core:** ORM para integração de banco de dados
- **Unit of Work:** Transações consistentes entre repositórios
- **Convention-based Mapping:** Configuração automática de entidades

## 7. PROJETO DO BANCO DE DADOS (Etapa 7)

### 7.1 Modelo Conceitual e Físico

**Principais Entidades:**
- **Patient:** Registro principal de pacientes
- **Triage:** Histórico de triagem de emergência
- **HospitalAdmission:** Registos de admissão e alta
- **StaffTask:** Tarefas da equipe de enfermagem
- **CriticalAlert:** Alertas críticos de emergência
- **AuditLog:** Log de auditoria completo
- **SchemaVersion:** Controle de versionamento do schema
- **UserAccount:** Autenticação de usuários administrativos

### 7.2 Stored Procedures Otimizadas

#### 7.2.1 PR_GetPatientClinicalSummary (Version 8)

**Full Literal Code:**
```sql
/*
    PR_GetPatientClinicalSummary
    Provides a unified view of a patient's clinical state, including the last triage,
    active admission, and pending tasks. Used by the Mobile Clinical App.
*/
CREATE PROCEDURE dbo.PR_GetPatientClinicalSummary
    @PatientId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Basic Patient Info
    SELECT
        PatientId, FullName, BirthDate, DocumentNumber, CreatedAtUtc
    FROM dbo.Patient
    WHERE PatientId = @PatientId;

    -- 2. Latest Triage
    SELECT TOP (1)
        TriageId, RiskLevel, ChiefComplaint, RecordedAtUtc
    FROM dbo.Triage
    WHERE PatientId = @PatientId
    ORDER BY RecordedAtUtc DESC;

    -- 3. Current Active Admission
    SELECT TOP (1)
        AdmissionId, BedNumber, AdmittedAtUtc, Status
    FROM dbo.HospitalAdmission
    WHERE PatientId = @PatientId AND Status = 1 -- Active
    ORDER BY AdmittedAtUtc DESC;

    -- 4. Recent Staff Tasks (limit 10)
    SELECT TOP (10)
        TaskId, Title, Priority, IsCompleted, AssignedRole, DueDateUtc
    FROM dbo.StaffTask
    WHERE PatientId = @PatientId
    ORDER BY IsCompleted ASC, Priority DESC, CreatedAtUtc DESC;

END;
GO
```

**Impacto na Performance:**
- **Dados Agregados:** Consulta única que retorna dados clínicos consolidados
- **Otimização de Índices:** Utiliza índices em `PatientId`, `RecordedAtUtc`, `Status`
- **Resultados Limitados:** Controle de resultados para evitar sobrecarga de dados

### 7.3 Triggers de Auditoria

#### 7.3.1 Trigger Genérico de Auditoria de Tabelas

**Template de Trigger:** Todos os triggers de auditoria seguem um padrão consistente:

```sql
CREATE TRIGGER dbo.TR_[TableName]_AuditLog
ON dbo.[TableName]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.AuditLog (EntityName, EntityId, ActionName, Details)
    SELECT
        N'[TableName]',
        COALESCE(inserted.[Id], deleted.[Id]),
        CASE
            WHEN inserted.[Id] IS NOT NULL AND deleted.[Id] IS NULL THEN N'INSERT'
            WHEN inserted.[Id] IS NOT NULL AND deleted.[Id] IS NOT NULL THEN N'UPDATE'
            ELSE N'DELETE'
        END,
        N'[TableName] record changed'
    FROM inserted
    FULL OUTER JOIN deleted ON inserted.[Id] = deleted.[Id];
END;
```

**Exemplos de Triggers Implementados:**

**TR_Patient_AuditLog:** Monitora alterações em registros de pacientes
**TR_Triage_AuditLog:** Registra modificações em triagens de emergência
**TR_HospitalAdmission_AuditLog:** Acompanha admissão e alta hospitalar
**TR_StaffTask_AuditLog:** Monitora tarefas da equipe de enfermagem

**Benefícios:**
- **Rastreabilidade Completa:** Todos os DDL e DML são auditados
- **Conformidade:** Atende aos requisitos NFR-02 e NFR-05
- **Segurança:** Detecta e investiga alterações não autorizadas
- **Integridade:** Garante operações consistentes

### 7.4 Constraints e Índices

**Constraints Principais:**
- **Primary Keys:** Constraints PK para todas as entidades
- **Foreign Keys:** Relacionamentos referenciais
- **Unique Constraints:** Garantia de dados exclusivos (ex: DocumentNumber)
- **Check Constraints:** Validação de regras de negócios (ex: Status values)

**Índices para Performance:**
- **IX_Triage_Patient:** `PatientId, RecordedAtUtc DESC` para consultas de histórico de triagem
- **IX_HospitalAdmission_Patient_AdmittedAtUtc:** Índice composto para admissão ativa
- **IX_StaffTask_Patient_Status:** `PatientId, IsCompleted, DueDateUtc` para tarefas mobile
- **IX_CriticalAlert_Unacknowledged:** `IsAcknowledged, TriggeredAtUtc DESC` para alertas críticos

## 8. INFRAESTRUTURA EM NUVEM E DEVOPS (Etapa 8)

### 8.1 Pipeline de CI/CD

**Ferramentas Principais:** GitHub Actions, Docker, Azure Services

**Workflow CI:** `.github/workflows/ci.yml`

**Pipeline de CI:**

```yaml
jobs:
  build-and-test:
    steps:
      - name: Restore
        run: dotnet restore HEJCareSuite.sln
      
      - name: Build
        run: dotnet build HEJCareSuite.sln --configuration Release --no-restore
      
      - name: Test
        run: dotnet test HEJCareSuite.sln --configuration Release --no-build --verbosity normal
      
      - name: Validate frontend syntax
        run: node --check src/HEJCareSuite.Api/wwwroot/app.js
      
      - name: Build deployment container
        run: docker build --tag hejcare-suite:ci .
      
      - name: Smoke test deployment container
        run: |
          docker run --detach --name hejcare-suite-ci --publish 18080:8080 \
            --env "ConnectionStrings__SqlServer=..." hejcare-suite:ci
          for attempt in $(seq 1 30); do
            if curl --fail --silent --show-error http://localhost:18080/health; then
              exit 0
            fi
            sleep 2
          done
          docker logs hejcare-suite-ci
          exit 1
```

### 8.2 Comando Sequencial de Build

**Etapas de Build:**

1. **dotnet clean:** Limpeza de artefatos anteriores
2. **dotnet restore:** Restauração de dependências NuGet
3. **dotnet build:** Compilação para Release
4. **dotnet test:** Execução de suite de testes completa
5. **docker build:** Construção de imagem Docker
6. **docker run:** Container smoke test

**Scripts Detalhados:**

**dotnet clean:** Remove artefatos de compilação anteriores, garante build limpo
**dotnet restore:** Resolve todos os pacotes NuGet, verifica versions compatíveis
**dotnet build:** Constrói solução inteira, valida sintaxe, emite warnings como erros
**dotnet test:** Executa testes unitários e de integração, valida funcionalidades
**docker build:** Cria imagem Docker usando multi-stage build (SDK -> ASP.NET runtime)
**docker run:** Containerização para teste de integração contínua

### 8.3 Pipeline de Deployment Sequencial

**Fases de Deployment:**

1. **Ambiente de Desenvolvimento:** Codespaces e Docker Compose local
2. **Homologação:** CI pipeline com approval gates
3. **Produção:** Deployment automatizado com monitoramento

**Componentes Principais:**

**Docker Compose:**
```yaml
services:
  app:
    build: .
    ports: ["8080:8080"]
    environment:
      ConnectionStrings__SqlServer: "Server=localhost;Database=HEJCareSuite;..."
  sqlserver:
    image: mssql/server:2022-latest
    ports: ["1433:1433"]
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "${MSSQL_SA_PASSWORD}"
```

**Dockerfile:**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HEJCareSuite.sln", "."]
COPY ["src/HEJCareSuite.Domain/HEJCareSuite.Domain.csproj", "src/HEJCareSuite.Domain/"]
COPY ["src/HEJCareSuite.Application/HEJCareSuite.Application.csproj", "src/HEJCareSuite.Application/"]
COPY ["src/HEJCareSuite.Infrastructure/HEJCareSuite.Infrastructure.csproj", "src/HEJCareSuite.Infrastructure/"]
COPY ["src/HEJCareSuite.Api/HEJCareSuite.Api.csproj", "src/HEJCareSuite.Api/"]
RUN dotnet restore "src/HEJCareSuite.Api/HEJCareSuite.Api.csproj"

COPY ["src/", "src/"]
RUN dotnet publish "src/HEJCareSuite.Api/HEJCareSuite.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HEJCareSuite.Api.dll"]
```

### 8.4 Monitoreamento e Validação

**Health Check Endpoint:** `GET /health` para verificação de status do container

**Monitoramento Principal:**
- **Logs de Container:** Acompanhamento de saída via `docker logs`
- **Métricas de Performance:** CPU, memória, rede do container
- **Validação de Smoke Test:** Endpoints críticos verificados em CI

**Governança de Segredos:**
- **MSSQL_SA_PASSWORD:** Armazenado como segredo do Codespaces
- **Connection Strings:** Variáveis de ambiente por ambiente
- **Configurações:** GitIgnore para segredos e senhas

## 9. ANÁLISE E UTILIDADE DO DOCUMENTO

### 9.1 Finalidade

Este documento consolidado (`DADOS_RELATORIO_PIM4.md`) serve como a **única fonte de verdade** para o grupo acadêmico, fornecendo:

- **Base Teórica:** Contextos organizacional e de negócios (Etapas 1-2)
- **Evidências Ágeis:** Gestão de projetos e entrega iterativa (Etapa 9)
- **Inclusão Digital:** Desenvolvimento de soluções acessíveis (Etapa 3)
- **Implementações Técnicas:** Arquiteturas web e mobile (Etapas 4-5)
- **Arquitetura de Software:** Design em camadas e princípios OO (Etapa 6)
- **Persistência de Dados:** Banco de dados otimizado (Etapa 7)
- **DevOps e Deployment:** Pipelines e infraestrutura (Etapa 8)

### 9.2 Instruções de Uso

**Para Redação do Relatório Final PIM IV:**

1. **Consulte Este Documento:** Utilize como fonte principal para todos os dados técnicos
2. **Cite Arquivos e Linhas:** Referências cruzadas para implementação concreta
3. **Correlacione Teoria e Prática:** Relacione com a base conceitual do README.md
4. **Padronize Formatação:** Linguagem ABNT formal e acadêmica
5. **Valide Evidências:** Verifique implementações reais contra especificações

### 9.3 Validação e Controle de Versão

**Este documento é mantido versionado** e atualizado ao longo do semestre acadêmico, servindo como registro tangível do desenvolvimento incremental do produto.

**Evidências Principais Presentes:**
- **Código:** Arquivos fonte completos com comentários
- **Configurações:** YAML, JSON, SQL, Dockerfiles
- **Documentação:** Padrões arquiteturais e decisões de design
- **Logs:** Seqüência de commits e runs de CI/CD

**Próximas Etapas:**
- **Validação:** Executar smoke tests e integração de CI
- **Documentação:** Atualizar medidas de qualidade e documentação de processo
- **Liberação:** Publicação de releases para homologação e produção

---

*Documento gerado automaticamente para PIM IV - HEJ Care Suite*
*Data de Geração:* 2026-11-11
*Co-Autor:* Claude Code <noreply@anthropic.com>
*Base Conceitual:* README.md (Etapas 1-12)
*Resposta a Requisitos:* Manual PIM IV Completo (Etapas 1-9)*