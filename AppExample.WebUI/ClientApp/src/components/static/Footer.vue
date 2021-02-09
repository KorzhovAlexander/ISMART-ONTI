<template>
  <div class="content-block">
    <footer class="footer">
      Copyright ELITE ONTI TEAM  © {{new Date().getFullYear()}}
    </footer>
  </div>
</template>

<style lang="scss">
.footer {
  @import "../../themes/generated/variables.base.scss";
  display: block;
  color: rgba($base-text-color, alpha($base-text-color) * 0.7);
  border-top: 1px solid rgba(0, 0, 0, 0.1);
  padding-top: 4px;
}
</style>
