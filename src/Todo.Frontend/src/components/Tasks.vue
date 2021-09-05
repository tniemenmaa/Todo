<template>
    <div class="drag-container drag-wrapper" v-drag-and-drop:options="dndOptions">
        <b-form-input v-model="search" placeholder="Search tasks"></b-form-input>
        
        <!-- Render all the base level tasks -->
        <vue-draggable-group v-model="tasks">
            <div class="tasks" >
                <task v-for="task in tasks" :key="task.id" :task="task" />
            </div>
        </vue-draggable-group>
        
        
    </div>
</template>
<script lang="js">
    import Vue from 'vue';
    //import axios from 'axios';
    import Task from './Task.vue'
    import { VueDraggableDirective } from 'vue-draggable'

    export default Vue.extend({
        directives: {
            dragAndDrop: VueDraggableDirective
        },
        components: {
            Task
        },
        data() {
            return {
                dndOptions: {
                    dropzoneSelector: '.tasks',
                    draggableSelector: '.task'
                },
                loading: false,
                tasks: [
                    {
                        id: 1,
                        summary: 'Go to store',
                        description: 'Purchase milk and bread',
                        createdAt: "2021-09-03T18:02:20",
                        deadlineAt: "2021-09-21T12:00:00",
                        priority: 1,
                        status: 0,
                        parentId: null,
                        children: null
                    },
                    {
                        id: 2,
                        summary: 'Purchase lederhosen',
                        description: 'It\'s almost oktoberfest season',
                        createdAt: "2021-09-03T18:02:20",
                        deadlineAt: null,
                        priority: 1,
                        status: 0,
                        parentId: 1,
                        children: null
                    }
                ],
                search: ''
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;
                this.tasks.push();

                this.tasks.push();
                /*axios.get("/api/tasks").then(d => {
                    console.log(d.data);
                })*/
            }
        }
    });
</script>
<style>
    
    ul {
        list-style: none;
        margin:0;
        padding: 0;
    }

    .task {
        margin: 10px;
        height: 100px;
        background: rgba(0,0,0, 0.4);
        transition: all 0.3s cubic-bezier(0.23, 1, 0.32, 1);
    }

    .item-dropzone-area {
        height: 6rem;
        background: #888;
        opacity: 0.8;
        animation-duration: 0.5s;
        animation-name: nodeInserted;
        margin-left: 0.6rem;
        margin-right: 0.6rem;
    }
</style>