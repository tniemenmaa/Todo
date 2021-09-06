
const sorting = {
    // Sorting options
    options: [ 
        {
            text: ''
        },
        {
            value: {
                field: 'id',
                ascending: true
            },
            text: 'id asc'
        },
        {
            value: {
                field: 'id',
                ascending: false
            },
            text: 'id desc'
        },
        {
            value: {
                field: 'priority',
                ascending: true
            },
            text: 'priority asc'
        },
        {
            value: {
                field: 'priority',
                ascending: false
            },
            text: 'priority desc'
        },
        {
            value: {
                field: 'summary',
                ascending: true
            },
            text: 'summary asc'
        },
        {
            value: {
                field: 'summary',
                ascending: false
            },
            text: 'summary desc'
        },
        {
            value: {
                field: 'createdAt',
                ascending: true
            },
            text: 'created asc'
        },
        {
            value: {
                field: 'createdAt',
                ascending: false
            },
            text: 'created desc'
        },
        {
            value: {
                field: 'deadlineAt',
                ascending: true
            },
            text: 'deadline asc'
        },
        {
            value: {
                field: 'deadlineAt',
                ascending: false
            },
            text: 'deadline desc'
        },
        {
            value: {
                field: 'status',
                ascending: true
            },
            text: 'status asc'
        },
        {
            value: {
                field: 'status',
                ascending: false
            },
            text: 'status desc'
        },
    ]
}

export default sorting;