'
' * Mostly copied from NETCDF4 source code.
' * refer : http://www.unidata.ucar.edu
' * 
' * Modified by iychoi@email.arizona.edu
' 

Namespace edu.arizona.cs.hdf5.structure

	Public Class DataBTree
		Private m_layout As Layout

        Public Sub New(layout As Layout)
			Me.m_layout = layout
		End Sub


        Public Overridable Function getChunkIterator([in] As BinaryReader, sb As Superblock) As DataChunkIterator
			Return New DataChunkIterator([in], sb, Me.m_layout)
		End Function
	End Class

End Namespace
