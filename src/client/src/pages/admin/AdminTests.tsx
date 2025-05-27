import { DeleteOutlined, EditOutlined, PlusOutlined, OrderedListOutlined, CopyOutlined, EyeOutlined } from '@ant-design/icons';
import { Button, Modal, Space, Table, message } from 'antd';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../store';
import { fetchTests } from '../../store/slices/testsSlice';
import type { Test } from '../../types';
import { TestForm } from '../../components/features/tests/TestForm';
import { DeleteTestModal } from '../../components/features/tests/DeleteTestModal';

const AdminTests = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { paged, status } = useSelector((state: RootState) => state.tests);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingTest, setEditingTest] = useState<Test | null>(null);
  const [deletingTest, setDeletingTest] = useState<Test | null>(null);

  useEffect(() => {
    dispatch(fetchTests({ pageIndex: 0, pageSize: 10 }));
  }, [dispatch]);

  const handleAdd = () => {
    setEditingTest(null);
    setIsModalVisible(true);
  };

  const handleEdit = (record: Test) => {
    setEditingTest(record);
    setIsModalVisible(true);
  };

  const handleDelete = (record: Test) => {
    setDeletingTest(record);
  };

  const handleFormSuccess = () => {
    setIsModalVisible(false);
    dispatch(fetchTests({ pageIndex: 0, pageSize: 10 }));
  };

  const handleDeleteSuccess = () => {
    setDeletingTest(null);
    dispatch(fetchTests({ pageIndex: 0, pageSize: 10 }));
  };

  const handleCopyId = (record: Test) => {
    navigator.clipboard.writeText(record.id);
    message.success('Test ID copied to clipboard');
  };

  const columns = [
    {
      title: 'Title',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: 'Actions',
      key: 'actions',
      render: (_: any, record: Test) => (
        <Space>
          <Link to={`/tests/${record.id}`} target="_blank" rel="noopener noreferrer">
            <Button
              type="text"
              icon={<EyeOutlined />}
              title="View Test"
            />
          </Link>
          <Link to={`/admin/questions/${record.id}`}>
            <Button
              type="text"
              icon={<OrderedListOutlined />}
              title="Manage Questions"
            />
          </Link>
          <Button
            type="text"
            icon={<CopyOutlined />}
            onClick={() => handleCopyId(record)}
            title="Copy Test ID"
          />
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
        <h1 style={{ margin: 0 }}>Tests</h1>
        <Button
          type="primary"
          icon={<PlusOutlined />}
          onClick={handleAdd}
        >
          Add Test
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
            dispatch(fetchTests({ pageIndex: page - 1, pageSize: 10 }));
          },
        }}
      />

      <Modal
        title={editingTest ? 'Edit Test' : 'Add Test'}
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        footer={null}
      >
        <TestForm
          initialValues={editingTest ?? undefined}
          onSuccess={handleFormSuccess}
          onCancel={() => setIsModalVisible(false)}
          loading={status === 'loading'}
        />
      </Modal>

      <DeleteTestModal
        test={deletingTest}
        onClose={() => setDeletingTest(null)}
        onSuccess={handleDeleteSuccess}
      />
    </div>
  );
};

export default AdminTests; 