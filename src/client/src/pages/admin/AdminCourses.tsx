import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Modal, Space, Table } from 'antd';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { CourseForm } from '../../components/features/courses/CourseForm';
import { DeleteCourseModal } from '../../components/features/courses/DeleteCourseModal';
import type { AppDispatch, RootState } from '../../store';
import { fetchCourses } from '../../store/slices/coursesSlice';
import type { CourseResponse } from '../../types';

const AdminCourses = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { paged, status } = useSelector((state: RootState) => state.courses);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingCourse, setEditingCourse] = useState<CourseResponse | null>(null);
  const [deletingCourse, setDeletingCourse] = useState<CourseResponse | null>(null);

  useEffect(() => {
    dispatch(fetchCourses({ pageIndex: 0, pageSize: 10 }));
  }, [dispatch]);

  const handleAdd = () => {
    setEditingCourse(null);
    setIsModalVisible(true);
  };

  const handleEdit = (record: CourseResponse) => {
    setEditingCourse(record);
    setIsModalVisible(true);
  };

  const handleDelete = (record: CourseResponse) => {
    setDeletingCourse(record);
  };

  const handleFormSuccess = () => {
    setIsModalVisible(false);
    dispatch(fetchCourses({ pageIndex: 0, pageSize: 10 }));
  };

  const handleDeleteSuccess = () => {
    setDeletingCourse(null);
    dispatch(fetchCourses({ pageIndex: 0, pageSize: 10 }));
  };

  const columns = [
    {
      title: 'Title',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: 'Description',
      dataIndex: 'description',
      key: 'description',
      ellipsis: true,
    },
    {
      title: 'Created At',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (date: string) => new Date(date).toLocaleDateString(),
    },
    {
      title: 'Actions',
      key: 'actions',
      render: (_: any, record: CourseResponse) => (
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

  return (
    <div>
      <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h1 style={{ margin: 0 }}>Courses</h1>
        <Button
          type="primary"
          icon={<PlusOutlined />}
          onClick={handleAdd}
        >
          Add Course
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
            dispatch(fetchCourses({ pageIndex: page - 1, pageSize: 10 }));
          },
        }}
      />

      <Modal
        title={editingCourse ? 'Edit Course' : 'Add Course'}
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        footer={null}
      >
        <CourseForm
          initialValues={editingCourse ?? undefined}
          onSuccess={handleFormSuccess}
          onCancel={() => setIsModalVisible(false)}
          loading={status === 'loading'}
        />
      </Modal>

      <DeleteCourseModal
        course={deletingCourse}
        onClose={() => setDeletingCourse(null)}
        onSuccess={handleDeleteSuccess}
      />
    </div>
  );
};

export default AdminCourses; 