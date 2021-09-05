<template>
    <b-card no-body class="task" :data-id="task.id">
        <b-card-header class="row align-items-start" header-tag="header" role="tab">
            <!-- Display data --> 
            <b-card-text class="col">{{ task.summary }}</b-card-text>
            <b-card-text class="col">{{ task.createdAt }}</b-card-text>
            <b-card-text class="col">{{ task.priority }}</b-card-text>
        </b-card-header>
    </b-card>
</template>
<script lang="js">
import Vue from 'vue';

export default Vue.extend({
    props: {
        task: Object
    }
});
</script>