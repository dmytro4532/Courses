import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Modal, Space, Table, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams, Link } from 'react-router-dom';
import api from '../../api/axios';
import type { AppDispatch, RootState } from '../../store';
import { fetchTopics } from '../../store/slices/topicsSlice';
import type { Topic, CourseResponse } from '../../types';
import LoadingSpinner from '../../components/common/LoadingSpinner';
import ErrorMessage from '../../components/common/ErrorMessage';
import { DeleteTopicModal } from '../../components/features/topics/DeleteTopicModal';
import { TopicForm } from '../../components/features/topics/TopicForm';

const { Title } = Typography;

const AdminTopics = () => {
    const { courseId } = useParams<{ courseId: string }>();
    const dispatch = useDispatch<AppDispatch>();
    const { paged, status } = useSelector((state: RootState) => state.topics);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [editingTopic, setEditingTopic] = useState<Topic | null>(null);
    const [deletingTopic, setDeletingTopic] = useState<Topic | null>(null);
    const [course, setCourse] = useState<CourseResponse | null>(null);
    const [courseLoading, setCourseLoading] = useState(true);
    const [courseError, setCourseError] = useState<string | null>(null);

    useEffect(() => {
        if (!courseId) return;
        setCourseLoading(true);
        setCourseError(null);
        api.get<CourseResponse>(`/api/courses/${courseId}`)
            .then((res) => setCourse(res.data))
            .catch((err) => setCourseError(err?.response?.data?.details || 'Failed to load course'))
            .finally(() => setCourseLoading(false));
    }, [courseId]);

    useEffect(() => {
        if (courseId) {
            dispatch(fetchTopics({ courseId, pageIndex: 0, pageSize: 10 }));
        }
    }, [dispatch, courseId]);

    const handleAdd = () => {
        setEditingTopic(null);
        setIsModalVisible(true);
    };

    const handleEdit = (record: Topic) => {
        setEditingTopic(record);
        setIsModalVisible(true);
    };

    const handleDelete = (record: Topic) => {
        setDeletingTopic(record);
    };

    const handleFormSuccess = () => {
        setIsModalVisible(false);
        if (courseId) {
            dispatch(fetchTopics({ courseId, pageIndex: 0, pageSize: 10 }));
        }
    };

    const handleDeleteSuccess = () => {
        setDeletingTopic(null);
        if (courseId) {
            dispatch(fetchTopics({ courseId, pageIndex: 0, pageSize: 10 }));
        }
    };

    const columns = [
        {
            title: 'Title',
            dataIndex: 'title',
            key: 'title',
        },
        {
            title: 'Content',
            dataIndex: 'content',
            key: 'content',
            ellipsis: true,
        },
        {
            title: 'Order',
            dataIndex: 'order',
            key: 'order',
        },
        {
            title: 'Actions',
            key: 'actions',
            render: (_: any, record: Topic) => (
                <Space>
                    <Button
                        type="text"
                        icon={<EditOutlined />}
                        onClick={() => handleEdit(record)}
                    />
                    <Button
                        type="text"
                        danger
                        icon={<DeleteOutlined />}
                        onClick={() => handleDelete(record)}
                    />
                </Space>
            ),
        },
    ];

    if (courseLoading) {
        return <LoadingSpinner />;
    }

    if (courseError || !course) {
        return <ErrorMessage message={courseError || 'Course not found'} />;
    }

    return (
        <div>
            <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Space direction="vertical" size={0}>
                    <Link to="/admin/courses">← Back to Courses</Link>
                    <Title level={2} style={{ margin: 0 }}>Topics - {course.title}</Title>
                </Space>
                <Button
                    type="primary"
                    icon={<PlusOutlined />}
                    onClick={handleAdd}
                >
                    Add Topic
                </Button>
            </div>

            <Table
                columns={columns}
                dataSource={paged?.items ?? []}
                rowKey="id"
                loading={status === 'loading'}
                pagination={{
                    current: (paged?.pageIndex ?? 0) + 1,
                    pageSize: paged?.pageSize ?? 10,
                    total: paged?.totalCount ?? 0,
                    onChange: (page) => {
                        if (courseId) {
                            dispatch(fetchTopics({ courseId, pageIndex: page - 1, pageSize: 10 }));
                        }
                    },
                }}
            />

            <Modal
                title={editingTopic ? 'Edit Topic' : 'Add Topic'}
                open={isModalVisible}
                onCancel={() => setIsModalVisible(false)}
                footer={null}
            >
                <TopicForm
                    initialValues={editingTopic ?? undefined}
                    courseId={courseId!}
                    onSuccess={handleFormSuccess}
                    onCancel={() => setIsModalVisible(false)}
                    loading={status === 'loading'}
                />
            </Modal>

            <DeleteTopicModal
                topic={deletingTopic}
                onClose={() => setDeletingTopic(null)}
                onSuccess={handleDeleteSuccess}
            />
        </div>
    );
};

export default AdminTopics; 