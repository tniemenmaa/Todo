<template>
    <div>
        <b-form-input v-model="search" placeholder="Search tasks"></b-form-input>
        
        <!-- Render all the base level tasks -->
        <draggable :list="tasks" :group="{ name: 'tasks' }" :data-parent="null" handle=".drag-handle" >
            <task v-for="task in tasks" :key="task.id" v-bind:task="task" :tasks="tasks" />
        </draggable>
        
        
    </div>
</template>
<script lang="js">
    import Vue from 'vue';
    //import axios from 'axios';
    import task from './Task.vue'
    import draggable from 'vuedraggable'

    export default Vue.extend({
        components: {
            task,
            draggable
        },
        data() {
            return {
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
                        children: [
                            {
                                id: 5,
                                summary: 'Go to store',
                                description: 'Purchase dozen eggs',
                                createdAt: "2021-09-03T18:02:20",
                                deadlineAt: "2021-09-21T12:00:00",
                                priority: 2,
                                status: 0,
                            }
                        ]
                    },
                    {
                        id: 2,
                        summary: 'Purchase lederhosen',
                        description: 'It\'s almost oktoberfest season',
                        createdAt: "2021-09-03T18:02:20",
                        deadlineAt: null,
                        priority: 1,
                        status: 0,
                        parentId: null,
                        children: [
                            {
                                id: 3,
                                summary: 'Purchase lederhosen 2',
                                description: 'It\'s almost oktoberfest season',
                                createdAt: "2021-09-03T18:02:20",
                                deadlineAt: null,
                                priority: 1,
                                status: 0,
                                parentId: 2
                            },
                            {
                                id: 4,
                                summary: 'Purchase lederhosen 3',
                                description: 'It\'s almost oktoberfest season',
                                createdAt: "2021-09-03T18:02:20",
                                deadlineAt: null,
                                priority: 1,
                                status: 0,
                                parentId: 2
                            }
                        ]
                    },
                    
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
                /*axios.get("/api/tasks").then(d => {
                    console.log(d.data);
                })*/
            },
            onTaskUpdated(task) {
                let i = this.tasks.findIndex(t => t.id === task.id);
                this.tasks[i] = task;
            }
            
        },
        computed: {
            // Tasks that have no parent
            mainTasks() {
                return this.tasks.filter(t => { return !t.parentId })
            }
        }
    });
</script>