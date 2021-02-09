<template>
  <div class="side-nav-outer-toolbar">
    <NavBar
        class="layout-header"
        :title="navTitle"
    />
    <DxScrollView ref="scrollViewRef" class="with-footer">
      <slot/>
      <slot name="footer"/>
    </DxScrollView>
  </div>
</template>

<script>
import DxScrollView from "devextreme-vue/scroll-view";
import NavBar from "./NavBar";

export default {
  name: "SideNavOuterToolbar",
  props: {
    navTitle: String,
    title: String,
    isXSmall: Boolean,
    isLarge: Boolean
  },
  data() {
    return {
      menuOpened: false,
      menuTemporaryOpened: false,
    }
  },
  components: {
    NavBar,
    DxScrollView,
  },
  watch: {
    $route() {
      this.scrollView.scrollTo(0);
    }
  },
  computed: {
    scrollView() {
      return this.$refs["scrollViewRef"].instance;
    }
  },
  methods: {},
}
</script>

<style lang="scss">
.side-nav-outer-toolbar {
  flex-direction: column;
  display: flex;
  height: 100%;
  width: 100%;
}

.container {
  height: 100%;
  flex-direction: column;
  display: flex;
}

.layout-header {
  z-index: 1501;
}

.layout-body {
  flex: 1;
  min-height: 0;
}

.content {
  flex-grow: 1;
}

#navigation-header {
  @import "../../themes/generated/variables.base.scss";
  background-color: $base-accent;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);

  .menu-button .dx-icon {
    color: $base-text-color;
  }

  .screen-x-small & {
    padding-left: 20px;
  }

  .dx-theme-generic & {
    padding-top: 10px;
    padding-bottom: 10px;
  }
}
</style>