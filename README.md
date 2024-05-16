# MiniHub

O MiniHub é um sistema de gerenciamento de arquivos para empresas e projetos. Com ele, é possível criar, editar, compartilhar e excluir arquivos, além de gerenciar usuários, permissões e flags. Tudo isso na rede interna da sua empresa ou projeto.

## Principáis tecnologias utilizadas

### .Net

O .NET é uma plataforma de desenvolvedor multiplataforma de código aberto gratuita para criar muitos tipos diferentes de aplicativos. O ASP.NET amplia a plataforma de desenvolvedor do .NET com ferramentas e bibliotecas específicas para a criação de aplicativos web. O C# é sua principal linguagem de programação, simples, moderna, com foco no objeto e de tipo seguro. 

Bibliotecas utilizadas no desenvolvimento:

| [ASP.Net](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi) | [Entity Framework](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore) | [AutoMapper](https://www.nuget.org/packages/AutoMapper) | [Bogus](https://www.nuget.org/packages/Bogus) | [FluentValidation](https://www.nuget.org/packages/FluentValidation) | [Serilog](https://www.nuget.org/packages/Serilog) | [xunit](https://www.nuget.org/packages/xunit) | [JWT](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
|:--:|:--:|:--:|:--:|:--:|:--:|:--:|:--:|
| ![ASP.Net](./docs/icons/net-framework.png) | ![ENtityFrameWork](./docs/icons/entity-framework.png) | ![AutoMapper](./docs/icons/automapper.png) | ![Bogus](./docs/icons/bogus.png) | ![FluentValidation](./docs/icons/fluentvalidation.png) | ![Serilog](./docs/icons/serilog.png) | ![xunit](./docs/icons/xunit.png) | ![JWT](./docs/icons/jwt.png)

### Next.js/React

O React é uma biblioteca JavaScript para criar interfaces de usuário (UI) reativas baseado no conceito de componentes, que são blocos de construção reutilizáveis que podem ser combinados para criar interfaces complexas. uma biblioteca popular para criar aplicativos web, incluindo sites, aplicativos móveis e aplicativos de desktop.

Bibliotecas utilizadas no desenvolvimento:

| [Next.js](https://nextjs.org/) | [React](https://react.dev/) | [MUI](https://www.npmjs.com/package/@mui/material) | [Material React Table](https://material-react-table.com/) | [Axios](https://www.npmjs.com/package/axios) | [ESLint](https://www.npmjs.com/package/eslint) | [Zod](https://zod.dev/) | [JWT-Decoder](https://www.npmjs.com/package/jwt-decode) | [React Hook Form](https://www.npmjs.com/package/react-hook-form) | [React Icons](https://react-icons.github.io/react-icons/)
|:--:|:--:|:--:|:--:|:--:|:--:|:--:|:--:|:--:|:--:
| ![Next.js](./docs/icons/nextjs.png) | ![React](./docs/icons/react.png) | ![MUI](./docs/icons/mui.png) | ![Material React Table](./docs/icons/material-react-table.png) | ![Axios](./docs/icons/axios.png) | ![ESLint](./docs/icons/eslint.png) | ![Zod](./docs/icons/zod.png) | ![JWT Decoder](./docs/icons/jwt.png) | ![react-hook-form](./docs/icons/react-hook-form.png) | ![React Icons](./docs/icons/react-icons.png)

## Principais Regras de Negócio

Usuários:

* Os Administradores e Supervisores podem visualizar todos os usuários;
* Os Administradores e Supervisores podem criar novos usuários;
* Um Usuário pode editar suas informações (nome, email, telefone, senha);
* Os Administradores podem deletar usuários;
* Os Administradores e Supervisores podem adicionar funções a um usuário;
* Os Administradores e Supervisores podem remover funções de um usuário;

Funções:

* Os Administradores e Supervisores podem visualizar todos os funções;
* Os Administradores e Supervisores podem criar novos funções;
* Os Administradores e Supervisores podem editar funções (nome);
* O Administrador pode deletar funções;

Flags:

* Um Colaborador pode visualizar flags;
* Um Colaborador pode criar flags;
* Um Colaborador pode editar flags (nome) que criou;
* Um Colaborador pode deletar flags que criou;

Diretórios:

* Um Colaborador pode visualizar diretórios que possuem seu cargo;
* Um Colaborador pode criar diretórios;
* Um Colaborador pode excluir diretórios que tenha criado;
* Um Colaborador pode adicionar funções a seus diretórios;
* Um Colaborador pode adicionar flags a seus diretórios;
* Um Colaborador pode compartilhar (link) de diretórios que possue acesso;

Arquivos:

* Um Colaborador pode visualizar arquivos que possuem seu cargo;
* Um Colaborador pode fazer upload de um arquivo;
* Um Colaborador pode baixar arquivos que possuem o seu cargo ou que criou;
* Um Colaborador pode adicionar funções a seus arquivos;
* Um Colaborador pode adicionar flags a seus arquivos;
* Um Colaborador pode compartilhar (link) de arquivos que possue acesso;

## Diagramas

Para modelar e representar a estrutura lógica dos dados, é utilizado o **Diagrama de Entidade-Relacionamento**, proporcionando uma visão clara e concisa das relações e da organização dos dados dentro do banco de dados.

![Diagrama de Entidade-Relacionamento](./docs/images/Entity%20Relationship%20Diagram.png)

O projeto esta sobre a Clean Architecture, visando testabilidade, flexíveis e separação de responsabilidades. Essa abordagem de arquitetura de software promove a separação clara e distinta das responsabilidades em diferentes camadas, permitindo que cada uma delas evolua de forma independente.

![Diagrama Clean Architecture](./docs/images/Clean%20Architecture.png)

Por fim, segue a interface das tabelas básicas (tabela geral e tabela de diretórios/arquivos) para o frontend.

```
interface Props<TData extends MRT_RowData> extends MRT_TableOptions<TData> {
  columns: MRT_ColumnDef<TData>[];
  data: TData[];
  setGlobalFilter?: Dispatch<SetStateAction<string>>;
  setColumnFilters?: Dispatch<SetStateAction<MRT_ColumnFiltersState>>;
  setSorting?: Dispatch<SetStateAction<MRT_SortingState>>;
  setPagination?: Dispatch<SetStateAction<MRT_PaginationState>>;
  rowCount?: number;
  initialSatate?: Partial<MRT_TableState<TData>>;
  state?: Partial<MRT_TableState<TData>>;
  enableRowSelection?: boolean;
  onSubmit?: () => void;
  isLoading?: () => boolean;
  title: string;
  toCreate?: string;
  toEdit?: string;
  handleDelete?: (id: string) => void;
}

export const useDefaultMaterialReactTable = <TData extends MRT_RowData>(
  { columns, data, ...props }: Props<TData>
) => {

  ...

  const table = useMaterialReactTable({
    columns,
    data,
    ...props,

    renderTopToolbarCustomActions: props.renderTopToolbarCustomActions ?? renderTopToolbarCustomActions,
    renderToolbarInternalActions: props.renderToolbarInternalActions ?? renderToolbarInternalActions,

    //#region setStates

    onGlobalFilterChange: props.setGlobalFilter,

    manualFiltering: props.manualFiltering ?? true,
    onColumnFiltersChange: props.setColumnFilters,

    manualSorting: props.manualSorting ?? true,
    onSortingChange: props.setSorting,

    enableRowSelection: props.enableRowSelection ?? true,
    getRowId: row => row.id ?? "",
    onRowSelectionChange: setRowSelection,

    manualPagination: props.manualPagination ?? true,
    onPaginationChange: props.setPagination,

    initialState: {
      showColumnFilters: true,
      density: "compact",
      ...props.initialState,
    },

    state: {
      isLoading: props.isLoading ? props.isLoading() : false,
      rowSelection,
      ...props.state,
    },

    //#endregion

    layoutMode: "grid",
    enableColumnResizing: props.enableColumnResizing ?? true,
    positionToolbarAlertBanner: "none",

    rowCount: props.rowCount,

    //#region Styles

    ...

    //#endregion

  });

  return (
    <>
      <DownloadExportDisplay
        fileName={props.title}
        head={table.getVisibleFlatColumns().map(c => c.columnDef).map(c => ({ id: c.id ?? "", value: c.header?.toString() ?? "" })).filter(c => c.id !== "mrt-row-select")}
        allRows={table.getRowModel().rows.map(r => r.original)}
        selectedRows={table.getSelectedRowModel().rows.map(r => r.original)}
      />
      <MaterialReactTable table={table} />
    </>
  );
};
```

```
interface Props<TData extends MRT_RowData> extends MRT_TableOptions<TData> {
  columns: MRT_ColumnDef<TData>[];
  data: TData[];
  setGlobalFilter?: Dispatch<SetStateAction<string>>;
  setColumnFilters?: Dispatch<SetStateAction<MRT_ColumnFiltersState>>;
  setSorting?: Dispatch<SetStateAction<MRT_SortingState>>;
  setPagination?: Dispatch<SetStateAction<MRT_PaginationState>>;
  rowCount?: number;
  initialSatate?: Partial<MRT_TableState<TData>>;
  state?: Partial<MRT_TableState<TData>>;
  onSubmit?: () => void;
  isLoading?: () => boolean;
  title: string;
  toUpload?: string;
  handleDelete?: (id: string) => void;
  handleDownload?: (row: TData) => void;
  handleUpload?: () => void;
}

export const useHubMaterialReactTable = <TData extends MRT_RowData>(
  { columns, data, ...props }: Props<TData>
) => {

  ...

  return useDefaultMaterialReactTable({
    columns,
    data,

    enablePagination: false,
    enableRowSelection: false,

    enableRowActions: true,

    layoutMode: 'grid-no-grow',

    renderRowActionMenuItems: ({ row, staticRowIndex, table }) => [
      <MenuItem key="download" value="download" onClick={() => props.handleDownload && props.handleDownload(row.original)}>
        <ListItemIcon sx={{ color: "inherit" }}>
          <FileDownload />
        </ListItemIcon>
        {"Download"}
      </MenuItem>,
      <MenuItem key="information" value="information">
        <ListItemIcon sx={{ color: "inherit" }}>
          <Info />
        </ListItemIcon>
        {"Information"}
      </MenuItem>
    ],

    renderTopToolbarCustomActions: renderTopToolbarCustomActions,
    renderToolbarInternalActions: renderToolbarInternalActions,

    initialState: {
      showColumnFilters: false,
      columnPinning: { left: ['mrt-row-actions', 'state'], right: ['city'] },
    },

    ...props,
  });
};
```


## Showcase

### Api
![Api - Swagger](./docs/screenshots/API%20Swagger%20-%2001.png)
![Api - Swagger](./docs/screenshots/API%20Swagger%20-%2002.png)
![Api - Swagger](./docs/screenshots/API%20Swagger%20-%2003.png)
![Api - Swagger](./docs/screenshots/API%20Swagger%20-%2004.png)
![Api - Swagger](./docs/screenshots/API%20Swagger%20-%2005.png)

### UI

![UI](./docs/screenshots/UI%2001.png)
![UI](./docs/screenshots/UI%2002.png)
![UI](./docs/screenshots/UI%2003.png)
![UI](./docs/screenshots/UI%2004.png)
![UI](./docs/screenshots/UI%2005.png)
![UI](./docs/screenshots/UI%2006.png)
![UI](./docs/screenshots/UI%2007.png)
![UI](./docs/screenshots/UI%2008.png)
![UI](./docs/screenshots/UI%2009.png)

![Sleep](./docs/gifs/sleep.webp)