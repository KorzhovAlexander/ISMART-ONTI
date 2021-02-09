<template>
  <div>
    <h2 class="content-block">Обратная связь от пользователей</h2>
    <DxDataGrid
        class="dx-card wide-card cut-dx-component"
        :ref="gridRefName"
        :data-source="dataSource"
        :show-borders="false"
        :focused-row-enabled="true"
        :column-auto-width="true"
        :column-hiding-enabled="true"
        :word-wrap-enabled="true"
        :row-alternation-enabled="false"
        :allow-column-reordering="true"
        :remote-operations="false"
        
        :repaint-changes-only="true"
        :highlight-changes="true"

        :allow-column-resizing="true"

        @toolbar-preparing="toolbarPreparing($event)"
        @exporting="onExporting"
    >
      <DxExport
          :enabled="true"
          :allow-export-selected-data="true"
      />
      <DxColumn
          :width="90"
          :hiding-priority="6"
          data-field="id"
          :allow-editing="false"
          :visible="false"
      />
      <DxColumn
          data-field="message"
          caption="Комментарий"
          :hiding-priority="8"
      />
      <DxColumn
          data-field="email"
          caption="email для связи"
          :hiding-priority="8"
      />

      <DxColumn
          data-field="userId"
          caption="Отправил"
          :hiding-priority="8"
      >
        <DxLookup :data-source="dataSourceUsers" value-expr="id" display-expr="userName"/>
      </DxColumn>

      <DxStateStoring
          :enabled="true"
          type="localStorage"
          :saving-timeout="0"
          storage-key="awdawdфцвфцвфцвaфвфцвфцвфцвdsasczdvsv"
      />
      <DxSummary>
        <DxGroupItem summary-type="count"/>
      </DxSummary>
      <DxSorting/>
      <DxPaging :page-size="50"/>
      <DxFilterRow :visible="true"/>
      <DxGroupPanel emptyPanelText=" " :visible="true"/>
      <DxLoadPanel :enabled="false"/>
      <DxSearchPanel :visible="true"/>
      <DxGrouping :auto-expand-all="false"/>
      <DxHeaderFilter :visible="true" :allow-search="true"/>
      <DxColumnChooser :enabled="true" mode="select"/>
      <DxScrolling mode="virtual" row-rendering-mode="virtual" column-rendering-mode="virtual"/>
    </DxDataGrid>

  </div>
</template>

<script>
import {DxButton,} from 'devextreme-vue'
import DxDataGrid, {
  DxColumn,
  DxFilterRow,
  DxPaging,
  DxSearchPanel,
  DxColumnChooser,
  DxSorting,
  DxHeaderFilter,
  DxStateStoring,
  DxScrolling,
  DxLoadPanel,
  DxGroupPanel,
  DxGrouping,
  DxSummary,
  DxGroupItem,
  DxLookup,
  DxExport,
} from "devextreme-vue/data-grid";

import * as AspNetData from "devextreme-aspnet-data-nojquery";
import {exportDataGrid} from 'devextreme/excel_exporter';
import saveAs from 'file-saver';
import ExcelJS from 'exceljs';

const dataSource = AspNetData.createStore({
  key: 'id',
  loadUrl: `/api/feedback`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = {withCredentials: true};
  },
});
const dataSourceUsers = AspNetData.createStore({
  key: 'id',
  loadUrl: `/api/user/all`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = {withCredentials: true};
  },
});

export default {
  name: "cart",
  data() {
    return {
      dataSource,
      dataSourceUsers,

      gridRefName: 'dataGrid',
    };
  },
  methods: {
    onExporting(e) {
      const workbook = new ExcelJS.Workbook();
      const worksheet = workbook.addWorksheet('P2W');
      exportDataGrid({
        component: e.component,
        worksheet: worksheet,
        autoFilterEnabled: true
      }).then(() => {
        // https://github.com/exceljs/exceljs#writing-xlsx
        workbook.xlsx.writeBuffer().then((buffer) => {
          saveAs(new Blob([buffer], {type: 'application/octet-stream'}), 'ELITE_Export.xlsx');
        });
      });
      e.cancel = true;
    },
    toolbarPreparing(e) {
      e.toolbarOptions.items.unshift(
          {
            location: 'after',
            widget: 'dxButton',
            locateInMenu: 'auto',
            showText: 'inMenu',
            options: {
              text: 'Обновить',
              hint: 'Обновить',
              icon: 'refresh',
              type: 'normal',
              stylingMode: 'contained',
              onClick: this.refreshDataGrid.bind(this)
            }
          })
    },
    async refreshDataGrid() {
      this.$refs[this.gridRefName].instance.refresh();
    },
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxFilterRow,
    DxPaging,
    DxSearchPanel,
    DxColumnChooser,
    DxSorting,
    DxHeaderFilter,
    DxStateStoring,
    DxScrolling,
    DxLoadPanel,
    DxGroupPanel,
    DxGrouping,
    DxSummary,
    DxGroupItem,
    DxButton,
    DxLookup,
    DxExport,
  },
}
</script>

<style scoped>

</style>